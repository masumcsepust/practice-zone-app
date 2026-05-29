using backend.Application.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;

namespace backend.Infrastructure.Speech;

public class AzureSpeechRecognitionService : ISpeechRecognitionService
{
    private readonly string _subscriptionKey;
    private readonly string _region;

    public AzureSpeechRecognitionService(IConfiguration config)
    {
        _subscriptionKey = config["Azure:Speech:SubscriptionKey"] ?? string.Empty;
        _region          = config["Azure:Speech:Region"]          ?? "southeastasia";
    }

    public async Task<string> RecognizeAsync(string audioFilePath, CancellationToken ct = default)
    {
        var speechConfig = SpeechConfig.FromSubscription(_subscriptionKey, _region);
        speechConfig.SpeechRecognitionLanguage = "ar-SA";

        using var audioConfig = BuildAudioConfig(audioFilePath);
        using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

        var result = await recognizer.RecognizeOnceAsync();

        return result.Reason switch
        {
            ResultReason.RecognizedSpeech => result.Text,
            ResultReason.NoMatch => throw new InvalidOperationException(
                "Speech could not be recognized. Ensure the audio contains clear Arabic speech."),
            ResultReason.Canceled => throw new InvalidOperationException(
                $"Recognition canceled: {CancellationDetails.FromResult(result).ErrorDetails}"),
            _ => throw new InvalidOperationException($"Unexpected recognition result: {result.Reason}")
        };
    }

    private static AudioConfig BuildAudioConfig(string audioFilePath)
    {
        var ext = Path.GetExtension(audioFilePath).ToLowerInvariant();

        if (ext == ".wav")
            return AudioConfig.FromWavFileInput(audioFilePath);

        var containerFormat = ext switch
        {
            ".mp3"  => AudioStreamContainerFormat.MP3,
            ".ogg"  => AudioStreamContainerFormat.OGG_OPUS,
            ".flac" => AudioStreamContainerFormat.FLAC,
            ".alaw" => AudioStreamContainerFormat.ALAW,
            ".mulaw"=> AudioStreamContainerFormat.MULAW,
            _       => AudioStreamContainerFormat.ANY
        };

        var compressedFormat = AudioStreamFormat.GetCompressedFormat(containerFormat);
        var pushStream = AudioInputStream.CreatePushStream(compressedFormat);

        var audioBytes = File.ReadAllBytes(audioFilePath);
        pushStream.Write(audioBytes);
        pushStream.Close();

        return AudioConfig.FromStreamInput(pushStream);
    }
}
