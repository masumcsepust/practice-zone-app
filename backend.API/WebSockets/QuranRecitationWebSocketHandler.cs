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
    private readonly ITajweedEngine _tajweedEngine;
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
        ITajweedEngine tajweedEngine,
        IServiceScopeFactory scopeFactory,
        ILogger<QuranRecitationWebSocketHandler> logger)
    {
        _sessionManager       = sessionManager;
        _speechService        = speechService;
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

        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        // Resolve userId from ?userId=N query param, or Bearer token value in Authorization header.
        // Full JWT validation belongs in auth middleware; this supports both patterns without
        // requiring a full auth stack to be wired up first.
        var userId = ResolveUserId(context);
        var connectionId    = _sessionManager.AddConnection(webSocket, userId);
        var clientIp        = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        _logger.LogInformation("WS connected [{ConnectionId}] user={UserId} ip={Ip}",
            connectionId, userId, clientIp);

        // ?ayahId=N  → Phase 3 pronunciation assessment
        // ?ayahId=N&tajweed=true → Phase 4 Tajweed engine
        // (no ayahId)            → Phase 2 transcription
        var ayahIdStr       = context.Request.Query["ayahId"].FirstOrDefault();
        var isPronunciation = int.TryParse(ayahIdStr, out var ayahId);
        var isTajweed       = isPronunciation
                           && context.Request.Query["tajweed"].FirstOrDefault() == "true";

        using var scope          = _scopeFactory.CreateScope();
        var streamingSessionRepo = scope.ServiceProvider.GetRequiredService<StreamingSessionRepository>();
        var streamingSession     = await streamingSessionRepo.CreateAsync(userId, connectionId);

        // ready=false means the session could not start (e.g. ayah not found)
        bool ready;
        if (isPronunciation)
            ready = await StartPronunciationSessionAsync(connectionId, ayahId, streamingSession.Id, scope, isTajweed);
        else
        {
            await StartTranscriptionSessionAsync(connectionId, streamingSession.Id);
            ready = true;
        }

        try
        {
            if (ready)
                await ReceiveLoopAsync(webSocket, connectionId, isPronunciation);
            else if (webSocket.State == WebSocketState.Open)
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.PolicyViolation, "Ayah not found", CancellationToken.None);
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

    private async Task<bool> StartPronunciationSessionAsync(
        string connectionId, int ayahId, int streamingSessionId, IServiceScope scope, bool isTajweed)
    {
        var ayahRepo = scope.ServiceProvider.GetRequiredService<IAyahRepository>();
        var ayah     = await ayahRepo.GetByIdAsync(ayahId);

        if (ayah is null)
        {
            _logger.LogWarning("[{ConnectionId}] Ayah {AyahId} not found.", connectionId, ayahId);
            var conn = _sessionManager.GetConnection(connectionId);
            if (conn is not null)
                await conn.SendTextAsync(
                    JsonSerializer.Serialize(new { type = "error", message = $"Ayah {ayahId} not found." }));
            return false;
        }

        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("[{ConnectionId}] {Mode} mode — ayah {AyahId}",
                connectionId, isTajweed ? "Tajweed" : "Pronunciation", ayahId);

        await _pronunciationService.StartSessionAsync(connectionId, ayah.ArabicText,
            onResult: async (dto) =>
            {
                var conn = _sessionManager.GetConnection(connectionId);
                if (conn is null) return;

                object payload = isTajweed
                    ? _tajweedEngine.Analyze(dto, ayah.ArabicText)
                    : dto;

                await conn.SendTextAsync(JsonSerializer.Serialize(payload, JsonOptions));

                using var innerScope = _scopeFactory.CreateScope();
                var repo = innerScope.ServiceProvider.GetRequiredService<PronunciationRepository>();
                await repo.SaveAsync(streamingSessionId, ayahId, dto);
            });

        return true;
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

    // ── UserId resolution ─────────────────────────────────────────────────

    private static int ResolveUserId(HttpContext context)
    {
        // 1. Explicit query param: ?userId=42
        if (int.TryParse(context.Request.Query["userId"].FirstOrDefault(), out var fromQuery))
            return fromQuery;

        // 2. Authorization: Bearer <numeric-id>  (placeholder until JWT middleware is wired)
        var bearer = context.Request.Headers.Authorization.FirstOrDefault();
        if (bearer?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
            && int.TryParse(bearer[7..].Trim(), out var fromBearer))
            return fromBearer;

        return 1; // anonymous / default user
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
            else if (result.MessageType == WebSocketMessageType.Text && result.Count > 0 && isPronunciation)
            {
                var text = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (text.Contains("end-of-audio"))
                    await _pronunciationService.CloseInputAsync(connectionId);
            }
        }
    }
}
