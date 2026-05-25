using System.Net.WebSockets;
using System.Text.Json;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Persistence.Repositories;

namespace backend.API.WebSockets;

public class QuranRecitationWebSocketHandler
{
    private readonly IWebSocketSessionManager _sessionManager;
    private readonly IRealtimeSpeechService _speechService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<QuranRecitationWebSocketHandler> _logger;

    public QuranRecitationWebSocketHandler(
        IWebSocketSessionManager sessionManager,
        IRealtimeSpeechService speechService,
        IServiceScopeFactory scopeFactory,
        ILogger<QuranRecitationWebSocketHandler> logger)
    {
        _sessionManager = sessionManager;
        _speechService  = speechService;
        _scopeFactory   = scopeFactory;
        _logger         = logger;
    }

    public async Task HandleAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        using var webSocket  = await context.WebSockets.AcceptWebSocketAsync();
        var userId           = 1; // Phase 3: resolve from JWT
        var connectionId     = _sessionManager.AddConnection(webSocket, userId);
        var clientIp         = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        _logger.LogInformation("WS connected [{ConnectionId}] user={UserId} ip={Ip}",
            connectionId, userId, clientIp);

        // Persist connection + create streaming session
        using var scope         = _scopeFactory.CreateScope();
        var streamingSessionRepo = scope.ServiceProvider.GetRequiredService<StreamingSessionRepository>();
        var streamingSession     = await streamingSessionRepo.CreateAsync(userId, connectionId);

        // Wire Azure → WebSocket: recognition results forwarded to client in real time
        await _speechService.StartSessionAsync(connectionId,
            onResult: async (type, text) =>
            {
                var conn = _sessionManager.GetConnection(connectionId);
                if (conn is null) return;

                var message = JsonSerializer.Serialize(
                    new RecognitionMessageDto { Type = type, Text = text });

                await conn.SendTextAsync(message);

                // Persist only final results — avoid DB writes per partial chunk
                if (type == "final")
                {
                    using var innerScope = _scopeFactory.CreateScope();
                    var repo = innerScope.ServiceProvider
                        .GetRequiredService<StreamingSessionRepository>();
                    await repo.SaveFinalResultAsync(streamingSession.Id, text);
                }
            });

        try
        {
            await ReceiveLoopAsync(webSocket, connectionId);
        }
        finally
        {
            await _speechService.StopSessionAsync(connectionId);
            _sessionManager.RemoveConnection(connectionId);

            using var cleanupScope = _scopeFactory.CreateScope();
            var cleanupRepo = cleanupScope.ServiceProvider
                .GetRequiredService<StreamingSessionRepository>();
            await cleanupRepo.CompleteAsync(connectionId, "Completed");

            _logger.LogInformation("WS disconnected [{ConnectionId}]", connectionId);
        }
    }

    private async Task ReceiveLoopAsync(WebSocket webSocket, string connectionId)
    {
        // 8 KB buffer — ~250ms of 16kHz 16-bit mono PCM audio per chunk
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
                await _speechService.WriteAudioAsync(connectionId, chunk);
            }
        }
    }
}
