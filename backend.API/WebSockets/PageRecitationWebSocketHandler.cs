using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using backend.Application.Interfaces;
using backend.Application.DTOs;
using backend.Persistence.Repositories;

namespace backend.API.WebSockets;

/// <summary>
/// WebSocket handler for page-by-page Quran recitation.
///
/// Endpoint: GET /ws/page-recitation?pageNumber=N[&amp;tajweed=true][&amp;userId=N]
///
/// Requires the page to have been pre-loaded via GET /api/quran/pages/{N} first.
///
/// Server → Client messages:
///   { type:"page-start",    pageNumber, totalAyahs, ayahNumber, arabicText }
///   { type:"ayah-ready",    ayahNumber, arabicText }
///   { type:"ayah-result",   ayahNumber, ...scores }
///   { type:"page-complete", totalAyahs, summary:[{ ayahNumber, score }] }
///   { type:"error",         message }
///
/// Client → Server messages:
///   Binary frames            → audio chunks for the current ayah
///   { type:"end-of-audio" }  → finalize and assess
///   { type:"next" }          → skip current ayah
///   { type:"repeat" }        → discard audio, re-announce same ayah
/// </summary>
public class PageRecitationWebSocketHandler
{
    private readonly IWebSocketSessionManager _sessionManager;
    private readonly IPronunciationAssessmentService _pronunciationService;
    private readonly ITajweedEngine _tajweedEngine;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PageRecitationWebSocketHandler> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy   = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public PageRecitationWebSocketHandler(
        IWebSocketSessionManager sessionManager,
        IPronunciationAssessmentService pronunciationService,
        ITajweedEngine tajweedEngine,
        IServiceScopeFactory scopeFactory,
        ILogger<PageRecitationWebSocketHandler> logger)
    {
        _sessionManager       = sessionManager;
        _pronunciationService = pronunciationService;
        _tajweedEngine        = tajweedEngine;
        _scopeFactory         = scopeFactory;
        _logger               = logger;
    }

    public async Task HandleAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        if (!int.TryParse(context.Request.Query["pageNumber"].FirstOrDefault(), out var pageNumber)
            || pageNumber < 1 || pageNumber > 604)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var isTajweed = context.Request.Query["tajweed"].FirstOrDefault() == "true";
        var userId    = ResolveUserId(context);

        using var webSocket  = await context.WebSockets.AcceptWebSocketAsync();
        var connectionId     = _sessionManager.AddConnection(webSocket, userId);

        _logger.LogInformation(
            "PageWS connected [{ConnectionId}] page={Page} tajweed={Tajweed}",
            connectionId, pageNumber, isTajweed);

        using var scope          = _scopeFactory.CreateScope();
        var streamingSessionRepo = scope.ServiceProvider.GetRequiredService<StreamingSessionRepository>();
        var streamingSession     = await streamingSessionRepo.CreateAsync(userId, connectionId);
        var ayahRepo             = scope.ServiceProvider.GetRequiredService<IAyahRepository>();

        var ayahs = await ayahRepo.GetByPageAsync(pageNumber);
        if (ayahs.Count == 0)
        {
            await SendJsonAsync(webSocket, new
            {
                type    = "error",
                message = $"Page {pageNumber} has no ayahs loaded. Fetch it first via GET /api/quran/pages/{pageNumber}."
            });
            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "No ayahs", CancellationToken.None);
            _sessionManager.RemoveConnection(connectionId);
            return;
        }

        try
        {
            await RunSessionAsync(webSocket, connectionId, pageNumber, ayahs, isTajweed, streamingSession.Id);
        }
        finally
        {
            _sessionManager.RemoveConnection(connectionId);

            using var cleanupScope = _scopeFactory.CreateScope();
            var cleanupRepo        = cleanupScope.ServiceProvider.GetRequiredService<StreamingSessionRepository>();
            await cleanupRepo.CompleteAsync(connectionId, "Completed");

            _logger.LogInformation("PageWS disconnected [{ConnectionId}]", connectionId);
        }
    }

    private async Task RunSessionAsync(
        WebSocket webSocket,
        string connectionId,
        int pageNumber,
        IReadOnlyList<backend.Domain.Entities.Ayah> ayahs,
        bool isTajweed,
        int streamingSessionId)
    {
        var currentIndex = 0;
        var audioBuffer  = new List<byte>(capacity: 256 * 1024);
        var recvBuffer   = new byte[8192];
        var summary      = new List<object>();

        await AnnounceAyahAsync(webSocket, pageNumber, ayahs, currentIndex, isStart: true);

        while (webSocket.State == WebSocketState.Open && currentIndex < ayahs.Count)
        {
            WebSocketReceiveResult result;
            try
            {
                result = await webSocket.ReceiveAsync(recvBuffer, CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning("PageWS recv error [{ConnectionId}]: {Message}", connectionId, ex.Message);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                if (webSocket.State == WebSocketState.Open)
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Binary && result.Count > 0)
            {
                audioBuffer.AddRange(recvBuffer[..result.Count]);
                continue;
            }

            if (result.MessageType != WebSocketMessageType.Text || result.Count == 0)
                continue;

            var text    = Encoding.UTF8.GetString(recvBuffer, 0, result.Count);
            var msgType = ParseMessageType(text);

            switch (msgType)
            {
                case "end-of-audio":
                    var mimeType = ParseMimeType(text);
                    var (advanced, nextIndex) = await AssessAndAdvanceAsync(
                        webSocket, connectionId, ayahs, currentIndex,
                        audioBuffer, isTajweed, streamingSessionId, summary, mimeType);

                    if (!advanced) break;

                    currentIndex = nextIndex;
                    if (currentIndex < ayahs.Count)
                        await AnnounceAyahAsync(webSocket, pageNumber, ayahs, currentIndex, isStart: false);
                    else
                        await SendJsonAsync(webSocket, new { type = "page-complete", totalAyahs = ayahs.Count, summary });
                    break;

                case "next":
                    audioBuffer.Clear();
                    currentIndex++;
                    if (currentIndex < ayahs.Count)
                        await AnnounceAyahAsync(webSocket, pageNumber, ayahs, currentIndex, isStart: false);
                    else
                        await SendJsonAsync(webSocket, new { type = "page-complete", totalAyahs = ayahs.Count, summary });
                    break;

                case "repeat":
                    audioBuffer.Clear();
                    await AnnounceAyahAsync(webSocket, pageNumber, ayahs, currentIndex, isStart: false);
                    break;

                default:
                    _logger.LogDebug("[{ConnectionId}] Unknown message: {Type}", connectionId, msgType);
                    break;
            }
        }
    }

    private async Task<(bool advanced, int newIndex)> AssessAndAdvanceAsync(
        WebSocket webSocket,
        string connectionId,
        IReadOnlyList<backend.Domain.Entities.Ayah> ayahs,
        int currentIndex,
        List<byte> audioBuffer,
        bool isTajweed,
        int streamingSessionId,
        List<object> summary,
        string mimeType = "audio/ogg")
    {
        var ayah       = ayahs[currentIndex];
        var audioBytes = audioBuffer.ToArray();
        audioBuffer.Clear();

        if (audioBytes.Length == 0)
        {
            await SendJsonAsync(webSocket, new { type = "error", message = "No audio received for this ayah." });
            return (false, currentIndex);
        }

        PronunciationResponseDto? pronResult;
        try
        {
            pronResult = await _pronunciationService.AssessOnceAsync(
                ayah.ArabicText, audioBytes, mimeType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{ConnectionId}] Assessment failed for ayah {AyahNumber}", connectionId, ayah.AyahNumber);
            await SendJsonAsync(webSocket, new { type = "error", message = "Assessment service error." });
            return (false, currentIndex);
        }

        if (pronResult is null)
        {
            await SendJsonAsync(webSocket, new { type = "error", message = "No speech detected — please try again." });
            return (false, currentIndex);
        }

        using (var innerScope = _scopeFactory.CreateScope())
        {
            var repo = innerScope.ServiceProvider.GetRequiredService<PronunciationRepository>();
            await repo.SaveAsync(streamingSessionId, ayah.Id, pronResult);
        }

        summary.Add(new { ayahNumber = ayah.AyahNumber, score = pronResult.PronunciationScore });

        object resultPayload = isTajweed
            ? _tajweedEngine.Analyze(pronResult, ayah.ArabicText)
            : pronResult;

        await SendAyahResultAsync(webSocket, ayah.AyahNumber, resultPayload);
        return (true, currentIndex + 1);
    }

    private Task AnnounceAyahAsync(
        WebSocket webSocket, int pageNumber,
        IReadOnlyList<backend.Domain.Entities.Ayah> ayahs,
        int index, bool isStart)
    {
        var ayah = ayahs[index];
        if (isStart)
        {
            return SendJsonAsync(webSocket, new
            {
                type       = "page-start",
                pageNumber,
                totalAyahs = ayahs.Count,
                ayahNumber = ayah.AyahNumber,
                arabicText = ayah.ArabicText
            });
        }

        return SendJsonAsync(webSocket, new
        {
            type       = "ayah-ready",
            ayahNumber = ayah.AyahNumber,
            arabicText = ayah.ArabicText
        });
    }

    private async Task SendAyahResultAsync(WebSocket webSocket, int ayahNumber, object payload)
    {
        var node = JsonSerializer.SerializeToNode(payload, JsonOptions)!.AsObject();
        node["type"]       = "ayah-result";
        node["ayahNumber"] = ayahNumber;
        var bytes = JsonSerializer.SerializeToUtf8Bytes(node, JsonOptions);
        await webSocket.SendAsync(bytes, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
    }

    private Task SendJsonAsync(WebSocket ws, object payload)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(payload, JsonOptions);
        return ws.SendAsync(bytes, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
    }

    private static string? ParseMessageType(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("type", out var prop))
                return prop.GetString();
        }
        catch { }
        return json.Trim().Trim('"');
    }

    private static string ParseMimeType(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("mimeType", out var prop))
                return prop.GetString() ?? "audio/ogg";
        }
        catch { }
        return "audio/ogg";
    }

    private static int ResolveUserId(HttpContext context)
    {
        if (int.TryParse(context.Request.Query["userId"].FirstOrDefault(), out var fromQuery))
            return fromQuery;
        return 1;
    }
}
