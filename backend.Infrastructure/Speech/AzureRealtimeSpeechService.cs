using System.Collections.Concurrent;
using backend.Application.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Speech;

public class AzureRealtimeSpeechService : IRealtimeSpeechService
{
    private readonly string _subscriptionKey;
    private readonly string _region;
    private readonly ILogger<AzureRealtimeSpeechService> _logger;

    // One recognizer kept alive per WebSocket connection
    private readonly ConcurrentDictionary<string, RecognizerSession> _sessions = new();

    private sealed record RecognizerSession(
        SpeechRecognizer Recognizer,
        PushAudioInputStream InputStream,
        AudioConfig AudioConfig);

    public AzureRealtimeSpeechService(IConfiguration config, ILogger<AzureRealtimeSpeechService> logger)
    {
        _subscriptionKey = config["Azure:Speech:SubscriptionKey"]
            ?? throw new InvalidOperationException("Azure:Speech:SubscriptionKey is not configured.");
        _region = config["Azure:Speech:Region"]
            ?? throw new InvalidOperationException("Azure:Speech:Region is not configured.");
        _logger = logger;
    }

    public async Task StartSessionAsync(
        string connectionId,
        Func<string, string, Task> onResult,
        CancellationToken ct = default)
    {
        var speechConfig = SpeechConfig.FromSubscription(_subscriptionKey, _region);
        speechConfig.SpeechRecognitionLanguage = "ar-SA";

        // ANY lets Azure auto-detect the container format (webm/opus from MediaRecorder)
        var audioFormat = AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.ANY);
        var pushStream  = AudioInputStream.CreatePushStream(audioFormat);
        var audioConfig = AudioConfig.FromStreamInput(pushStream);

        var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

        // Partial result — fires continuously while the user is speaking
        recognizer.Recognizing += async (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Result.Text))
            {
                _logger.LogDebug("[{Id}] Partial: {Text}", connectionId, e.Result.Text);
                await onResult("partial", e.Result.Text);
            }
        };

        // Final result — fires when Azure detects end of utterance
        recognizer.Recognized += async (_, e) =>
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech
                && !string.IsNullOrWhiteSpace(e.Result.Text))
            {
                _logger.LogInformation("[{Id}] Final: {Text}", connectionId, e.Result.Text);
                await onResult("final", e.Result.Text);
            }
        };

        recognizer.Canceled += (_, e) =>
            _logger.LogWarning("[{Id}] Recognition canceled: {Details}",
                connectionId, e.ErrorDetails);

        _sessions[connectionId] = new RecognizerSession(recognizer, pushStream, audioConfig);

        // Start continuous recognition — stays alive for the entire connection
        await recognizer.StartContinuousRecognitionAsync();

        _logger.LogInformation("[{Id}] Azure recognizer started.", connectionId);
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

        _logger.LogInformation("[{Id}] Azure recognizer stopped and disposed.", connectionId);
    }
}
