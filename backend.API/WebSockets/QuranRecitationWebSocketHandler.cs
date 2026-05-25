using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Persistence.Repositories;

namespace backend.API.WebSockets;

public class QuranRecitationWebSocketHandler
{
    private readonly IWebSocketSessionManager _sessionManager;
    private readonly IRealtimeSpeechService _speechService;
    private readonly IPronunciationAssessmentService _pronunciationService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<QuranRecitationWebSocketHandler> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy   = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public QuranRecitationWebSocketHandler(
        IWebSocketSessionManager sessionManager,
        IRealtimeSpeechService speechService,
        IPronunciationAssessmentService pronunciationService,
        IServiceScopeFactory scopeFactory,
        ILogger<QuranRecitationWebSocketHandler> logger)
    {
        _sessionManager      = sessionManager;
        _speechService       = speechService;
        _pronunciationService = pronunciationService;
        _scopeFactory        = scopeFactory;
        _logger              = logger;
    }

    public async Task HandleAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var userId          = 1; // Phase 4: resolve from JWT
        var connectionId    = _sessionManager.AddConnection(webSocket, userId);
        var clientIp        = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        _logger.LogInformation("WS connected [{ConnectionId}] user={UserId} ip={Ip}",
            connectionId, userId, clientIp);

        using var scope              = _scopeFactory.CreateScope();
        var streamingSessionRepo     = scope.ServiceProvider.GetRequiredService<StreamingSessionRepository>();
        var streamingSession         = await streamingSessionRepo.CreateAsync(userId, connectionId);

        // ?ayahId=N routes to Phase 3 pronunciation assessment; omitting it falls back to Phase 2
        var ayahIdStr          = context.Request.Query["ayahId"].FirstOrDefault();
        var isPronunciation    = int.TryParse(ayahIdStr, out var ayahId);

        if (isPronunciation)
            await StartPronunciationSessionAsync(connectionId, ayahId, streamingSession.Id, scope);
        else
            await StartTranscriptionSessionAsync(connectionId, streamingSession.Id);

        try
        {
            await ReceiveLoopAsync(webSocket, connectionId, isPronunciation);
        }
        finally
        {
            if (isPronunciation)
                await _pronunciationService.StopSessionAsync(connectionId);
            else
                await _speechService.StopSessionAsync(connectionId);

            _sessionManager.RemoveConnection(connectionId);

            using var cleanupScope = _scopeFactory.CreateScope();
            var cleanupRepo        = cleanupScope.ServiceProvider.GetRequiredService<StreamingSessionRepository>();
            await cleanupRepo.CompleteAsync(connectionId, "Completed");

            _logger.LogInformation("WS disconnected [{ConnectionId}]", connectionId);
        }
    }

    // ── Phase 3: pronunciation assessment ──────────────────────────────────

    private async Task StartPronunciationSessionAsync(
        string connectionId, int ayahId, int streamingSessionId, IServiceScope scope)
    {
        var ayahRepo = scope.ServiceProvider.GetRequiredService<IAyahRepository>();
        var ayah     = await ayahRepo.GetByIdAsync(ayahId);

        if (ayah is null)
        {
            _logger.LogWarning("[{ConnectionId}] Ayah {AyahId} not found — closing.", connectionId, ayahId);
            var conn = _sessionManager.GetConnection(connectionId);
            if (conn is not null)
                await conn.SendTextAsync(
                    JsonSerializer.Serialize(new { type = "error", message = $"Ayah {ayahId} not found." }));
            return;
        }

        _logger.LogInformation("[{ConnectionId}] Pronunciation mode — ayah {AyahId}", connectionId, ayahId);

        await _pronunciationService.StartSessionAsync(connectionId, ayah.ArabicText,
            onResult: async (dto) =>
            {
                var conn = _sessionManager.GetConnection(connectionId);
                if (conn is null) return;

                await conn.SendTextAsync(JsonSerializer.Serialize(dto, JsonOptions));

                using var innerScope = _scopeFactory.CreateScope();
                var repo = innerScope.ServiceProvider.GetRequiredService<PronunciationRepository>();
                await repo.SaveAsync(streamingSessionId, ayahId, dto);
            });
    }

    // ── Phase 2: plain transcription ───────────────────────────────────────

    private async Task StartTranscriptionSessionAsync(string connectionId, int streamingSessionId)
    {
        await _speechService.StartSessionAsync(connectionId,
            onResult: async (type, text) =>
            {
                var conn = _sessionManager.GetConnection(connectionId);
                if (conn is null) return;

                var message = JsonSerializer.Serialize(
                    new RecognitionMessageDto { Type = type, Text = text });
                await conn.SendTextAsync(message);

                if (type == "final")
                {
                    using var innerScope = _scopeFactory.CreateScope();
                    var repo = innerScope.ServiceProvider.GetRequiredService<StreamingSessionRepository>();
                    await repo.SaveFinalResultAsync(streamingSessionId, text);
                }
            });
    }

    // ── Shared receive loop ────────────────────────────────────────────────

    private async Task ReceiveLoopAsync(WebSocket webSocket, string connectionId, bool isPronunciation)
    {
        var buffer = new byte[8192];

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result;
            try
            {
                result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning("WS error [{ConnectionId}]: {Message}", connectionId, ex.Message);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Binary && result.Count > 0)
            {
                var chunk = buffer[..result.Count];
                if (isPronunciation)
                    await _pronunciationService.WriteAudioAsync(connectionId, chunk);
                else
                    await _speechService.WriteAudioAsync(connectionId, chunk);
            }
        }
    }
}
