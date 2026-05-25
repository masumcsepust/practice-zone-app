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
        _subscriptionKey = config["Azure:Speech:SubscriptionKey"]
            ?? throw new InvalidOperationException("Azure:Speech:SubscriptionKey is not configured.");
        _region = config["Azure:Speech:Region"]
            ?? throw new InvalidOperationException("Azure:Speech:Region is not configured.");
        _phonemeAnalysis = phonemeAnalysis;
        _logger          = logger;
    }

    public async Task StartSessionAsync(
        string connectionId,
        string referenceText,
        Func<PronunciationResponseDto, Task> onResult,
        CancellationToken ct = default)
    {
        var speechConfig = SpeechConfig.FromSubscription(_subscriptionKey, _region);
        speechConfig.SpeechRecognitionLanguage = "ar-SA";

        var pronunciationConfig = new PronunciationAssessmentConfig(
            referenceText: referenceText,
            gradingSystem: GradingSystem.HundredMark,
            granularity:   Granularity.Phoneme,
            enableMiscue:  true);

        // PCM 16kHz 16-bit mono — no GStreamer dependency on Linux
        var audioFormat = AudioStreamFormat.GetWaveFormatPCM(16000, 16, 1);
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

        recognizer.Canceled += (_, e) =>
            _logger.LogWarning("[{Id}] Pronunciation recognition canceled: {Details}",
                connectionId, e.ErrorDetails);

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

    public async Task StopSessionAsync(string connectionId)
    {
        if (!_sessions.TryRemove(connectionId, out var session)) return;

        await session.Recognizer.StopContinuousRecognitionAsync();
        session.InputStream.Close();
        session.Recognizer.Dispose();
        session.AudioConfig.Dispose();

        _logger.LogInformation("[{Id}] Azure pronunciation recognizer stopped and disposed.", connectionId);
    }
}
