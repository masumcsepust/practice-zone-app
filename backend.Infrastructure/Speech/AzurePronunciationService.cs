using System.Collections.Concurrent;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.PronunciationAssessment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Speech;

public class AzurePronunciationService : IPronunciationAssessmentService
{
    private readonly string _subscriptionKey;
    private readonly string _region;
    private readonly IPhonemeAnalysisService _phonemeAnalysis;
    private readonly ILogger<AzurePronunciationService> _logger;

    private readonly ConcurrentDictionary<string, RecognizerSession> _sessions = new();

    private sealed record RecognizerSession(
        SpeechRecognizer Recognizer,
        PushAudioInputStream InputStream,
        AudioConfig AudioConfig);

    public AzurePronunciationService(
        IConfiguration config,
        IPhonemeAnalysisService phonemeAnalysis,
        ILogger<AzurePronunciationService> logger)
    {
        // Allow empty key — StartSessionAsync will return an error to the caller
        // rather than crashing at startup (legacy WebSocket path, superseded by HTTP check-pronunciation).
        _subscriptionKey = config["Azure:Speech:SubscriptionKey"] ?? string.Empty;
        _region          = config["Azure:Speech:Region"]          ?? "southeastasia";
        _phonemeAnalysis = phonemeAnalysis;
        _logger          = logger;
    }

    public async Task StartSessionAsync(
        string connectionId,
        string referenceText,
        Func<PronunciationResponseDto, Task> onResult,
        Func<string, Task>? onError = null,
        CancellationToken ct = default)
    {
        var speechConfig = SpeechConfig.FromSubscription(_subscriptionKey, _region);
        speechConfig.SpeechRecognitionLanguage = "ar-SA";

        var pronunciationConfig = new PronunciationAssessmentConfig(
            referenceText: referenceText,
            gradingSystem: GradingSystem.HundredMark,
            granularity:   Granularity.Phoneme,
            enableMiscue:  true);

        // ANY lets Azure auto-detect the container format (webm/opus from MediaRecorder)
        var audioFormat = AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.ANY);
        var pushStream  = AudioInputStream.CreatePushStream(audioFormat);
        var audioConfig = AudioConfig.FromStreamInput(pushStream);

        var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
        pronunciationConfig.ApplyTo(recognizer);

        recognizer.Recognized += async (_, e) =>
        {
            if (e.Result.Reason != ResultReason.RecognizedSpeech
                || string.IsNullOrWhiteSpace(e.Result.Text))
                return;

            // SDK object provides sentence-level scores
            var azureScores = PronunciationAssessmentResult.FromResult(e.Result);

            // DetailedResultJson provides word + phoneme data (SDK Words property has no phonemes)
            var detailedJson = e.Result.Properties.GetProperty(
                PropertyId.SpeechServiceResponse_JsonResult);

            var words        = _phonemeAnalysis.ParseWords(detailedJson);
            var weakPhonemes = words
                .SelectMany(w => w.Phonemes)
                .Where(p => p.IsWeak)
                .ToList();

            var dto = new PronunciationResponseDto
            {
                RecognizedText     = e.Result.Text,
                PronunciationScore = azureScores.PronunciationScore,
                AccuracyScore      = azureScores.AccuracyScore,
                FluencyScore       = azureScores.FluencyScore,
                CompletenessScore  = azureScores.CompletenessScore,
                Words              = words,
                WeakPhonemes       = weakPhonemes
            };

            _logger.LogInformation("[{Id}] Pronunciation: score={Score:F1} words={Count} weakPhonemes={Weak}",
                connectionId, azureScores.PronunciationScore, words.Count, weakPhonemes.Count);

            await onResult(dto);
        };

        recognizer.Canceled += async (_, e) =>
        {
            _logger.LogWarning("[{Id}] Pronunciation recognition canceled: {Reason} — {Details}",
                connectionId, e.Reason, e.ErrorDetails);

            if (onError is not null)
            {
                var msg = e.Reason == CancellationReason.Error
                    ? $"Speech service error: {e.ErrorDetails}"
                    : "কোনো কথা শোনা যায়নি। দয়া করে আবার চেষ্টা করুন।";
                await onError(msg);
            }
        };

        _sessions[connectionId] = new RecognizerSession(recognizer, pushStream, audioConfig);
        await recognizer.StartContinuousRecognitionAsync();

        _logger.LogInformation("[{Id}] Azure pronunciation recognizer started (ref={Len} chars).",
            connectionId, referenceText.Length);
    }

    public Task WriteAudioAsync(string connectionId, byte[] chunk)
    {
        if (_sessions.TryGetValue(connectionId, out var session))
            session.InputStream.Write(chunk);
        return Task.CompletedTask;
    }

    public Task CloseInputAsync(string connectionId)
    {
        if (_sessions.TryGetValue(connectionId, out var session))
            session.InputStream.Close();
        return Task.CompletedTask;
    }

    public async Task StopSessionAsync(string connectionId)
    {
        if (!_sessions.TryRemove(connectionId, out var session)) return;

        await session.Recognizer.StopContinuousRecognitionAsync();
        session.InputStream.Close();
        session.Recognizer.Dispose();
        session.AudioConfig.Dispose();

        _logger.LogInformation("[{Id}] Azure pronunciation recognizer stopped and disposed.", connectionId);
    }

    // ── One-shot letter assessment ────────────────────────────────────────────

    public async Task<PronunciationResponseDto?> AssessOnceAsync(
        string            referenceText,
        byte[]            audioBytes,
        string            mimeType,
        CancellationToken ct = default)
    {
        var speechConfig = SpeechConfig.FromSubscription(_subscriptionKey, _region);
        speechConfig.SpeechRecognitionLanguage = "ar-SA";
        // Generous silence budget — ayahs take 5-15 s; don't cut off mid-recitation
        speechConfig.SetProperty(PropertyId.SpeechServiceConnection_InitialSilenceTimeoutMs, "8000");
        speechConfig.SetProperty(PropertyId.SpeechServiceConnection_EndSilenceTimeoutMs,     "3000");

        var paConfig = new PronunciationAssessmentConfig(
            referenceText: referenceText,
            gradingSystem: GradingSystem.HundredMark,
            granularity:   Granularity.Phoneme,
            enableMiscue:  true);

        var baseMime    = mimeType.Split(';')[0].Trim();
        var audioConfig = BuildAudioConfig(audioBytes, baseMime);

        _logger.LogInformation("AssessOnce mimeType={Mime} bytes={Bytes}", baseMime, audioBytes.Length);

        using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
        paConfig.ApplyTo(recognizer);

        var result = await recognizer.RecognizeOnceAsync();
        audioConfig.Dispose();

        if (result.Reason == ResultReason.Canceled)
        {
            var cancel = CancellationDetails.FromResult(result);
            _logger.LogWarning(
                "AssessOnce CANCELED: Reason={Reason} Code={Code} Details={Details}",
                cancel.Reason, cancel.ErrorCode, cancel.ErrorDetails);
            return null;
        }

        _logger.LogInformation("AssessOnce reason={Reason} text={Text}", result.Reason, result.Text);

        if (string.IsNullOrWhiteSpace(result.Text))
            return null;   // NoMatch — no speech or couldn't match reference

        var paResult     = PronunciationAssessmentResult.FromResult(result);
        var detailedJson = result.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult);
        var words        = _phonemeAnalysis.ParseWords(detailedJson);
        var weakPhonemes = words.SelectMany(w => w.Phonemes).Where(p => p.IsWeak).ToList();

        return new PronunciationResponseDto
        {
            RecognizedText     = result.Text,
            PronunciationScore = paResult.PronunciationScore,
            AccuracyScore      = paResult.AccuracyScore,
            FluencyScore       = paResult.FluencyScore,
            CompletenessScore  = paResult.CompletenessScore,
            Words              = words,
            WeakPhonemes       = weakPhonemes
        };
    }

    private static AudioConfig BuildAudioConfig(byte[] audioBytes, string mimeType)
    {
        // SDK 1.50 compressed formats: OGG_OPUS, MP3, FLAC, ANY (no WEBM_OPUS / MPEG4_AAC)
        var m = mimeType.ToLowerInvariant();
        AudioStreamContainerFormat? compressed = m switch
        {
            var s when s.Contains("ogg") || s.Contains("opus") => AudioStreamContainerFormat.OGG_OPUS,
            var s when s.Contains("mp3")                       => AudioStreamContainerFormat.MP3,
            var s when s.Contains("flac")                      => AudioStreamContainerFormat.FLAC,
            var s when s.Contains("webm") || s.Contains("mp4")
                    || s.Contains("mpeg") || s.Contains("aac") => AudioStreamContainerFormat.ANY,
            _                                                  => null
        };

        if (compressed.HasValue)
        {
            var fmt  = AudioStreamFormat.GetCompressedFormat(compressed.Value);
            var push = AudioInputStream.CreatePushStream(fmt);
            push.Write(audioBytes);
            push.Close();
            return AudioConfig.FromStreamInput(push);
        }

        // WAV / raw PCM — strip 44-byte RIFF header if present
        var push2  = AudioInputStream.CreatePushStream();
        var isRiff = audioBytes.Length > 44
                     && audioBytes[0] == 'R' && audioBytes[1] == 'I'
                     && audioBytes[2] == 'F' && audioBytes[3] == 'F';
        push2.Write(isRiff ? audioBytes[44..] : audioBytes);
        push2.Close();
        return AudioConfig.FromStreamInput(push2);
    }
}
