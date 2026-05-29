using backend.Application.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Speech;

public class ArabicLetterTtsService : IArabicLetterTtsService
{
    private readonly string _subscriptionKey;
    private readonly string _region;
    private readonly ILogger<ArabicLetterTtsService> _logger;

    public ArabicLetterTtsService(IConfiguration config, ILogger<ArabicLetterTtsService> logger)
    {
        // Allow empty key — SynthesizeAsync will throw at call time instead of crashing startup
        _subscriptionKey = config["Azure:Speech:SubscriptionKey"] ?? string.Empty;
        _region          = config["Azure:Speech:Region"]          ?? "southeastasia";
        _logger          = logger;
    }

    public async Task<byte[]> SynthesizeAsync(string arabicText, CancellationToken ct = default)
    {
        var speechConfig = SpeechConfig.FromSubscription(_subscriptionKey, _region);
        speechConfig.SpeechSynthesisLanguage = "ar-SA";
        speechConfig.SpeechSynthesisVoiceName = "ar-SA-ZariyahNeural";
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio16Khz32KBitRateMonoMp3);

        using var synthesizer = new SpeechSynthesizer(speechConfig, null);

        var ssml = $"""
            <speak version="1.0" xmlns="http://www.w3.org/2001/10/synthesis" xml:lang="ar-SA">
              <voice name="ar-SA-ZariyahNeural">
                <prosody rate="slow">{arabicText}</prosody>
              </voice>
            </speak>
            """;

        using var result = await synthesizer.SpeakSsmlAsync(ssml);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            return result.AudioData;

        _logger.LogError("TTS synthesis failed for '{Text}': {Reason}", arabicText, result.Reason);
        throw new InvalidOperationException($"TTS synthesis failed: {result.Reason}");
    }
}
