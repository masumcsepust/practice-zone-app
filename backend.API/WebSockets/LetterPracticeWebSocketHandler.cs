using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using backend.Application.Interfaces;

namespace backend.API.WebSockets;

public class LetterPracticeWebSocketHandler
{
    private readonly IWebSocketSessionManager _sessionManager;
    private readonly IPronunciationAssessmentService _pronunciationService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LetterPracticeWebSocketHandler> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy   = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public LetterPracticeWebSocketHandler(
        IWebSocketSessionManager sessionManager,
        IPronunciationAssessmentService pronunciationService,
        IServiceScopeFactory scopeFactory,
        ILogger<LetterPracticeWebSocketHandler> logger)
    {
        _sessionManager       = sessionManager;
        _pronunciationService = pronunciationService;
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

        using var webSocket  = await context.WebSockets.AcceptWebSocketAsync();
        var connectionId     = _sessionManager.AddConnection(webSocket, userId: 1);

        var letterIdStr = context.Request.Query["letterId"].FirstOrDefault();
        if (!int.TryParse(letterIdStr, out var letterId))
        {
            await SendErrorAsync(connectionId, "Missing or invalid letterId query parameter.");
            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Bad request", CancellationToken.None);
            _sessionManager.RemoveConnection(connectionId);
            return;
        }

        using var scope  = _scopeFactory.CreateScope();
        var letterRepo   = scope.ServiceProvider.GetRequiredService<IArabicLetterRepository>();
        var letter       = await letterRepo.GetByIdAsync(letterId);

        if (letter is null)
        {
            await SendErrorAsync(connectionId, $"Letter {letterId} not found.");
            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Letter not found", CancellationToken.None);
            _sessionManager.RemoveConnection(connectionId);
            return;
        }

        _logger.LogInformation("[{ConnectionId}] Letter practice — {Letter} ({Name})",
            connectionId, letter.Letter, letter.NameArabic);

        await _pronunciationService.StartSessionAsync(connectionId, letter.NameArabic,
            onError: async (msg) =>
            {
                await SendErrorAsync(connectionId, msg);
            },
            onResult: async (dto) =>
            {
                var conn = _sessionManager.GetConnection(connectionId);
                if (conn is null) return;

                bool isCorrect = dto.AccuracyScore >= 60.0;

                string feedback = dto.AccuracyScore switch
                {
                    >= 95 => $"Outstanding! \"{letter.NameEnglish}\" pronunciation is perfect. 🌟",
                    >= 85 => $"Excellent! Very good pronunciation. Your {letter.MakhrajType} articulation is spot-on.",
                    >= 75 => $"Good job! Keep practicing to refine your pronunciation.",
                    >= 60 => $"Acceptable. Focus on {letter.MakhrajType}: {letter.MakhrajDescription}.",
                    >= 40 => $"Keep trying. Listen to the audio and speak slowly and clearly.",
                    _     => $"Don't give up! Listen to \"{letter.NameEnglish}\" again and try once more."
                };

                var result = new
                {
                    type               = "letter-result",
                    letter             = letter.Letter,
                    letterName         = letter.NameArabic,
                    letterNameBn       = letter.NameBangla,
                    recognizedText     = dto.RecognizedText,
                    pronunciationScore = dto.PronunciationScore,
                    accuracyScore      = dto.AccuracyScore,
                    isCorrect,
                    feedback,
                    makhrajHint        = letter.MakhrajDescriptionBn
                };

                await conn.SendTextAsync(JsonSerializer.Serialize(result, JsonOptions));
            });

        try
        {
            await ReceiveLoopAsync(webSocket, connectionId);
        }
        finally
        {
            await _pronunciationService.StopSessionAsync(connectionId);
            _sessionManager.RemoveConnection(connectionId);
            _logger.LogInformation("[{ConnectionId}] Letter practice disconnected", connectionId);
        }
    }

    private async Task ReceiveLoopAsync(WebSocket webSocket, string connectionId)
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
                _logger.LogWarning("[{ConnectionId}] WS error: {Message}", connectionId, ex.Message);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Binary && result.Count > 0)
            {
                await _pronunciationService.WriteAudioAsync(connectionId, buffer[..result.Count]);
            }
            else if (result.MessageType == WebSocketMessageType.Text && result.Count > 0)
            {
                var text = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (text.Contains("end-of-audio"))
                    await _pronunciationService.CloseInputAsync(connectionId);
            }
        }
    }

    private async Task SendErrorAsync(string connectionId, string message)
    {
        var conn = _sessionManager.GetConnection(connectionId);
        if (conn is null) return;
        var payload = JsonSerializer.Serialize(new { type = "error", message }, JsonOptions);
        await conn.SendTextAsync(payload);
    }
}
