using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;

namespace backend.API.WebSockets;

/// <summary>
/// WebSocket handler for /ws/letter?letterId={id}.
///
/// Flow:
///   1. Client streams audio as binary WebSocket frames.
///   2. Client sends "mime:{type}|end-of-audio" or just "end-of-audio".
///   3. Handler buffers all audio, then calls AssessOnceAsync — a one-shot
///      Azure Pronunciation Assessment that compares the spoken audio against
///      the letter's Arabic name (e.g. "بَاء") as reference text.
///   4. Sends back a "letter-result" JSON frame and closes.
///
/// Why one-shot, not streaming:
///   Streaming continuous recognition fires Canceled (timeout) on short single-
///   letter sounds. RecognizeOnceAsync with Pronunciation Assessment handles
///   sub-second audio reliably.
/// </summary>
public class LetterPracticeWebSocketHandler
{
    private readonly IWebSocketSessionManager        _sessionManager;
    private readonly IPronunciationAssessmentService _pronunciationService;
    private readonly IServiceScopeFactory            _scopeFactory;
    private readonly ILogger<LetterPracticeWebSocketHandler> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy   = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public LetterPracticeWebSocketHandler(
        IWebSocketSessionManager        sessionManager,
        IPronunciationAssessmentService pronunciationService,
        IServiceScopeFactory            scopeFactory,
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

        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var connectionId    = _sessionManager.AddConnection(webSocket, userId: 1);

        // ── Validate letterId ────────────────────────────────────────────────
        var letterIdStr = context.Request.Query["letterId"].FirstOrDefault();
        if (!int.TryParse(letterIdStr, out var letterId))
        {
            await SendErrorAsync(webSocket, "Missing or invalid letterId query parameter.");
            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Bad request", CancellationToken.None);
            _sessionManager.RemoveConnection(connectionId);
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var letterRepo  = scope.ServiceProvider.GetRequiredService<IArabicLetterRepository>();
        var letter      = await letterRepo.GetByIdAsync(letterId);

        if (letter is null)
        {
            await SendErrorAsync(webSocket, $"Letter {letterId} not found.");
            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Letter not found", CancellationToken.None);
            _sessionManager.RemoveConnection(connectionId);
            return;
        }

        _logger.LogInformation("[{Id}] Letter practice WS — {Letter} ({Name})",
            connectionId, letter.Letter, letter.NameArabic);

        try
        {
            await ReceiveAndEvaluateAsync(webSocket, connectionId, letter, context.RequestAborted);
        }
        finally
        {
            _sessionManager.RemoveConnection(connectionId);
            _logger.LogInformation("[{Id}] Letter practice WS disconnected.", connectionId);
        }
    }

    // ── Core loop ─────────────────────────────────────────────────────────────

    private async Task ReceiveAndEvaluateAsync(
        WebSocket    webSocket,
        string       connectionId,
        ArabicLetter letter,
        CancellationToken ct)
    {
        using var audioBuffer = new MemoryStream();
        var recvBuffer        = new byte[8192];
        var mimeType          = "audio/webm"; // overridden by "mime:<type>|end-of-audio"

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result;
            try
            {
                result = await webSocket.ReceiveAsync(recvBuffer, ct);
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning("[{Id}] WS error: {Msg}", connectionId, ex.Message);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }

            // Binary frame → accumulate audio
            if (result.MessageType == WebSocketMessageType.Binary && result.Count > 0)
            {
                audioBuffer.Write(recvBuffer, 0, result.Count);
                continue;
            }

            // Text frame → parse mime prefix and wait for end-of-audio
            if (result.MessageType == WebSocketMessageType.Text && result.Count > 0)
            {
                var text = Encoding.UTF8.GetString(recvBuffer, 0, result.Count);

                if (text.Contains("mime:"))
                {
                    var mimeStart = text.IndexOf("mime:", StringComparison.Ordinal) + 5;
                    var mimeEnd   = text.IndexOf('|', mimeStart);
                    if (mimeEnd > mimeStart)
                        mimeType = text[mimeStart..mimeEnd];
                }

                if (!text.Contains("end-of-audio")) continue;

                // ── All audio received — run one-shot Pronunciation Assessment ──
                var audioBytes = audioBuffer.ToArray();
                _logger.LogInformation("[{Id}] Audio complete: {Bytes} bytes, mime={Mime}",
                    connectionId, audioBytes.Length, mimeType);

                PronunciationResponseDto? paResult;
                try
                {
                    paResult = await _pronunciationService.AssessOnceAsync(
                        referenceText: letter.NameArabic,
                        audioBytes:    audioBytes,
                        mimeType:      mimeType,
                        ct:            ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[{Id}] Pronunciation assessment failed.", connectionId);
                    await SendErrorAsync(webSocket, "উচ্চারণ যাচাই ব্যর্থ হয়েছে। দয়া করে আবার চেষ্টা করুন।");
                    break;
                }

                var response = BuildResult(letter, paResult);
                var json     = JsonSerializer.Serialize(response, JsonOptions);
                var encoded  = Encoding.UTF8.GetBytes(json);
                await webSocket.SendAsync(encoded, WebSocketMessageType.Text, true, ct);

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
                break;
            }
        }
    }

    // ── Result builder ────────────────────────────────────────────────────────

    private static object BuildResult(ArabicLetter letter, PronunciationResponseDto? pa)
    {
        // No speech detected
        if (pa is null)
        {
            return new
            {
                type               = "letter-result",
                letter             = letter.Letter,
                letterName         = letter.NameArabic,
                letterNameBn       = letter.NameBangla,
                recognizedText     = "",
                pronunciationScore = 0.0,
                accuracyScore      = 0.0,
                isCorrect          = false,
                feedback           = "Nothing heard. Please speak louder and closer to the mic.",
                feedbackBn         = "কিছু শোনা যায়নি। মাইকের কাছে আরও জোরে বলার চেষ্টা করুন।",
                makhrajHint        = letter.MakhrajDescriptionBn
            };
        }

        var heard     = pa.RecognizedText.Trim();
        var weakCount = pa.WeakPhonemes.Count;

        // Azure ar-SA ignores Arabic short vowels (harakat) — "alf" and "أَلِف" score the same.
        // Penalise proportionally when weak phonemes are detected or completeness is low.
        var rawScore = pa.PronunciationScore;
        var penalisedScore = AdjustScore(rawScore, pa.AccuracyScore, pa.CompletenessScore, weakCount,
                                         pa.Words.Sum(w => w.Phonemes.Count));

        var isCorrect = penalisedScore >= 60 && pa.CompletenessScore >= 60;

        string feedback, feedbackBn;

        if (!isCorrect && !string.IsNullOrWhiteSpace(heard))
        {
            // Speech recognized but score is too low — wrong letter or incorrect pronunciation
            var weakHint = weakCount > 0
                ? $" (weak sound{(weakCount > 1 ? "s" : "")}: {string.Join(", ", pa.WeakPhonemes.Select(p => p.Phoneme))})"
                : string.Empty;
            feedback   = $"Incorrect. I heard \"{heard}\" but it doesn't match {letter.NameEnglish} ({letter.Letter}).{weakHint} Try again.";
            feedbackBn = $"ভুল! আমি \"{heard}\" শুনেছি কিন্তু {letter.NameEnglish} ({letter.Letter})-এর সাথে মেলেনি। আবার চেষ্টা করুন।";
        }
        else
        {
            var weakHint = weakCount > 0
                ? $" Weak phoneme(s): {string.Join(", ", pa.WeakPhonemes.Select(p => p.Phoneme))}."
                : string.Empty;

            (feedback, feedbackBn) = penalisedScore switch
            {
                >= 90 => (
                    $"Excellent! Your pronunciation is near-perfect.{weakHint}",
                    $"চমৎকার! আপনার উচ্চারণ প্রায় নিখুঁত।"),
                >= 75 => (
                    $"Good job! Minor refinements needed.{weakHint}",
                    $"খুব ভালো! সামান্য উন্নতি প্রয়োজন।"),
                >= 60 => (
                    $"Acceptable. Keep practising for a cleaner sound.{weakHint}",
                    $"মোটামুটি ঠিক। আরও স্পষ্ট উচ্চারণের জন্য অনুশীলন করুন।"),
                _ => (
                    $"Partially correct. Focus on the makhraj point.{weakHint}",
                    "আংশিক সঠিক। মাখরাজ পয়েন্টে মনোযোগ দিন।")
            };
        }

        return new
        {
            type               = "letter-result",
            letter             = letter.Letter,
            letterName         = letter.NameArabic,
            letterNameBn       = letter.NameBangla,
            recognizedText     = heard,
            pronunciationScore = Math.Round(penalisedScore, 1),
            accuracyScore      = pa.AccuracyScore,
            isCorrect,
            feedback,
            feedbackBn,
            makhrajHint        = letter.MakhrajDescriptionBn
        };
    }

    /// <summary>
    /// Azure ar-SA ignores Arabic short vowel harakat, so "alf" scores identically to "alif".
    /// We penalise the raw score based on weak phoneme ratio and completeness gap.
    /// </summary>
    private static double AdjustScore(
        double rawScore, double accuracyScore, double completenessScore,
        int weakPhonemes, int totalPhonemes)
    {
        var score = rawScore;

        // Penalise for low completeness (missing phonemes/syllables)
        if (completenessScore < 100)
            score = score * (completenessScore / 100.0) * 0.9 + score * 0.1;

        // Penalise for weak phonemes: each weak phoneme reduces score by 8 points
        if (totalPhonemes > 0 && weakPhonemes > 0)
        {
            var weakRatio = (double)weakPhonemes / totalPhonemes;
            score -= weakRatio * 40;
        }

        // Also weight in accuracy score
        score = score * 0.7 + accuracyScore * 0.3;

        return Math.Max(0, Math.Min(100, score));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static async Task SendErrorAsync(WebSocket webSocket, string message)
    {
        if (webSocket.State != WebSocketState.Open) return;
        var payload = JsonSerializer.Serialize(new { type = "error", message }, JsonOptions);
        var encoded = Encoding.UTF8.GetBytes(payload);
        await webSocket.SendAsync(encoded, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
