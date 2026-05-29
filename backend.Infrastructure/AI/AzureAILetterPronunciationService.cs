using System.Text.Json;
using Azure;
using Azure.AI.Inference;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace backend.Infrastructure.AI;

/// <summary>
/// HTTP-based letter pronunciation checker.
///
/// Flow:
///   1. Transcribe the uploaded audio via Azure Speech SDK one-shot recognition.
///   2. Send the transcript + target letter details to Azure AI Inference
///      for bilingual scoring and feedback.
///   3. Return a structured <see cref="LetterPronunciationResultDto"/>.
///
/// This replaces the unreliable WebSocket / continuous-recognition path
/// that produced "EndOfStream" cancellation errors.
/// </summary>
public sealed class AzureAILetterPronunciationService : ILetterPronunciationService
{
    private readonly string   _speechKey;
    private readonly string   _speechRegion;
    private readonly Kernel?  _kernel;        // null when Azure:AI keys are not configured
    private readonly ILogger<AzureAILetterPronunciationService> _logger;

    // ── Evaluation prompt ────────────────────────────────────────────────────
    private const string SystemPrompt = """
        You are an expert Arabic Tajweed and phonetics teacher evaluating a student's pronunciation.
        The student attempted to pronounce one Arabic letter.
        You receive: the target letter info AND the text that a speech-recognition engine heard.

        Evaluate how accurately the student pronounced the target letter and respond
        ONLY with a valid JSON object (no markdown, no extra text):
        {
          "pronunciationScore": 0-100,
          "accuracyScore": 0-100,
          "isCorrect": true/false,
          "feedback": "encouraging English feedback (1-2 sentences)",
          "feedbackBn": "same feedback in Bangla (বাংলায়, 1-2 sentences)",
          "makhrajHint": "one specific articulation tip based on the letter's makhraj (Bangla preferred)"
        }

        Scoring guide:
        - 90-100: Perfect or near-perfect pronunciation of the letter.
        - 75-89 : Good — minor articulation imperfections.
        - 60-74 : Acceptable, clear room for improvement.
        - 40-59 : Partially correct.
        - 0-39  : Unrecognizable or wrong letter entirely.
        Set isCorrect = true when pronunciationScore >= 60.
        If no speech was detected (truly silent), give a score of 0, set isCorrect false, and advise
        the student to speak louder and closer to the microphone, and briefly explain the correct pronunciation.
        If speech was detected but unclear (Azure could not transcribe it), give at least 50-65 score and
        set isCorrect true — single Arabic letter sounds are very short and often hard for STT engines.
        Be encouraging; frame all criticism as positive guidance.
        """;

    public AzureAILetterPronunciationService(
        IConfiguration config,
        ILogger<AzureAILetterPronunciationService> logger)
    {
        _logger = logger;

        // Azure Speech (may be empty — transcription gracefully skipped if so)
        _speechKey    = config["Azure:Speech:SubscriptionKey"] ?? string.Empty;
        _speechRegion = config["Azure:Speech:Region"]         ?? "southeastasia";

        // Azure AI Inference — build Kernel only when credentials are present
        var endpoint = config["Azure:AI:Endpoint"];
        var apiKey   = config["Azure:AI:ApiKey"];

        if (!string.IsNullOrWhiteSpace(endpoint) && !string.IsNullOrWhiteSpace(apiKey))
        {
            var modelId = config["Azure:AI:ModelId"] ?? "gpt-4o";
            var client  = new ChatCompletionsClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            _kernel = Kernel.CreateBuilder()
                .AddAzureAIInferenceChatCompletion(modelId, chatClient: client)
                .Build();
        }
        else
        {
            _logger.LogWarning("Azure:AI:Endpoint/ApiKey not configured — pronunciation AI evaluation disabled.");
            _kernel = null;
        }
    }

    // ── ILetterPronunciationService ──────────────────────────────────────────

    public async Task<LetterPronunciationResultDto> CheckAsync(
        ArabicLetterDto   letter,
        byte[]            audioBytes,
        string            mimeType  = "audio/wav",
        CancellationToken ct        = default)
    {
        _logger.LogInformation(
            "Pronunciation check for letter '{Letter}' ({Name}), audio={Bytes} bytes, mime={Mime}",
            letter.Letter, letter.NameEnglish, audioBytes.Length, mimeType);

        // Step 1 — always transcribe
        var transcript = await TranscribeAsync(audioBytes, mimeType, letter, ct);
        _logger.LogInformation("Transcript: '{Text}'", transcript);

        if (_kernel is null)
        {
            _logger.LogWarning("Azure AI is not configured. Performing local fallback evaluation.");
            
            if (string.IsNullOrWhiteSpace(transcript))
            {
                return new LetterPronunciationResultDto(
                    IsCorrect:      false,
                    Score:          0,
                    AccuracyScore:  0,
                    RecognizedText: string.Empty,
                    Feedback:       "No speech was detected. Please try again.",
                    FeedbackBn:     "কোনো কথা শোনা যায়নি। দয়া করে আবার চেষ্টা করুন।",
                    MakhrajHint:    letter.MakhrajDescriptionBn);
            }

            // Speech detected but Azure couldn't transcribe — give benefit of the doubt
            if (transcript == "[SPEECH_DETECTED_BUT_UNCLEAR]")
            {
                return new LetterPronunciationResultDto(
                    IsCorrect:      true,
                    Score:          60,
                    AccuracyScore:  55,
                    RecognizedText: string.Empty,
                    Feedback:       "Speech was detected! Single letter sounds are short — keep practising clearly.",
                    FeedbackBn:     "আওয়াজ শোনা গেছে! একটি বর্ণের শব্দ খুব সংক্ষিপ্ত — স্পষ্টভাবে অনুশীলন চালিয়ে যান।",
                    MakhrajHint:    letter.MakhrajDescriptionBn);
            }

            // Simple fallback: check if the transcript contains the letter, Arabic name, or English name
            var cleanTranscript = transcript.TrimEnd('.').Trim();
            bool matches = cleanTranscript.Contains(letter.Letter) ||
                           cleanTranscript.Contains(letter.NameArabic) ||
                           cleanTranscript.Contains(letter.NameEnglish, StringComparison.OrdinalIgnoreCase);

            return new LetterPronunciationResultDto(
                IsCorrect:      matches,
                Score:          matches ? 95 : 40,
                AccuracyScore:  matches ? 90 : 30,
                RecognizedText: transcript,
                Feedback:       matches ? "Good job! That sounded correct." : "That didn't quite match the target letter.",
                FeedbackBn:     matches ? "চমৎকার! আপনার উচ্চারণ সঠিক হয়েছে।" : "উচ্চারণটি ঠিক লক্ষ্য বর্ণের মতো শোনায়নি।",
                MakhrajHint:    letter.MakhrajDescriptionBn);
        }

        // Step 2 — evaluate with Azure AI (LLM)
        return await EvaluateAsync(letter, transcript, ct);
    }

    // ── Transcription ────────────────────────────────────────────────────────

    private async Task<string> TranscribeAsync(
        byte[]          audioBytes,
        string          mimeType,
        ArabicLetterDto letter,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_speechKey))
        {
            _logger.LogWarning("Azure Speech SubscriptionKey is empty — skipping transcription.");
            return string.Empty;
        }

        try
        {
            var speechConfig = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
            speechConfig.SpeechRecognitionLanguage = "ar-SA";

            // Shorter timeouts help with isolated letter sounds (single syllable utterances)
            speechConfig.SetProperty(PropertyId.SpeechServiceConnection_InitialSilenceTimeoutMs, "5000");
            speechConfig.SetProperty(PropertyId.SpeechServiceConnection_EndSilenceTimeoutMs,     "1000");

            // Normalize mime type — strip codec params before format detection
            var baseMime    = mimeType.Split(';')[0].Trim();
            var audioConfig = BuildAudioConfig(audioBytes, baseMime);
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            // Phrase list — tell Azure Speech to expect this specific letter's Arabic names
            // This significantly improves recognition of short isolated letter sounds
            var phrases = PhraseListGrammar.FromRecognizer(recognizer);
            phrases.AddPhrase(letter.Letter);
            phrases.AddPhrase(letter.NameArabic);
            if (!string.IsNullOrWhiteSpace(letter.Transliteration))
                phrases.AddPhrase(letter.Transliteration);

            // One-shot recognition — no streaming, no EndOfStream issues
            var result = await recognizer.RecognizeOnceAsync();
            audioConfig.Dispose();

            _logger.LogInformation("Azure Speech reason: {Reason}", result.Reason);

            return result.Reason switch
            {
                ResultReason.RecognizedSpeech when !string.IsNullOrWhiteSpace(result.Text)
                    => result.Text,
                ResultReason.NoMatch
                    => "[SPEECH_DETECTED_BUT_UNCLEAR]",
                _ => string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Speech transcription failed — evaluating with empty transcript.");
            return string.Empty;
        }
    }

    private static AudioConfig BuildAudioConfig(byte[] audioBytes, string mimeType)
    {
        var isCompressed =
            mimeType.Contains("webm",  StringComparison.OrdinalIgnoreCase) ||
            mimeType.Contains("ogg",   StringComparison.OrdinalIgnoreCase) ||
            mimeType.Contains("opus",  StringComparison.OrdinalIgnoreCase) ||
            mimeType.Contains("mpeg",  StringComparison.OrdinalIgnoreCase) ||
            mimeType.Contains("mp3",   StringComparison.OrdinalIgnoreCase);

        if (isCompressed)
        {
            var format = AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.ANY);
            var push   = AudioInputStream.CreatePushStream(format);
            push.Write(audioBytes);
            push.Close();
            return AudioConfig.FromStreamInput(push);
        }

        // WAV — strip 44-byte RIFF header before feeding raw PCM
        var push2 = AudioInputStream.CreatePushStream();
        var isRiff = audioBytes.Length > 44
                     && audioBytes[0] == 'R' && audioBytes[1] == 'I'
                     && audioBytes[2] == 'F' && audioBytes[3] == 'F';
        push2.Write(isRiff ? audioBytes[44..] : audioBytes);
        push2.Close();
        return AudioConfig.FromStreamInput(push2);
    }

    // ── AI evaluation ────────────────────────────────────────────────────────

    private async Task<LetterPronunciationResultDto> EvaluateAsync(
        ArabicLetterDto   letter,
        string            transcript,
        CancellationToken ct)
    {
        var chat    = _kernel!.GetRequiredService<IChatCompletionService>();
        var history = new ChatHistory(SystemPrompt);

        var transcriptLine = transcript switch
        {
            "[SPEECH_DETECTED_BUT_UNCLEAR]"
                => "Speech recognition result: [Azure Speech detected audio/voice but could not clearly transcribe the letter — the student likely attempted the pronunciation but the sound was too short or had an accent variation. Give the benefit of the doubt and score leniently (at least 50-65) unless there is strong evidence of incorrect articulation.]",
            { Length: 0 }
                => "Speech recognition result: [no speech detected — student may have spoken too quietly or briefly]",
            _
                => $"Speech recognition result (what the student said): \"{transcript}\""
        };

        history.AddUserMessage($"""
            Target letter   : {letter.Letter}  ({letter.NameEnglish} / {letter.NameArabic} / {letter.NameBangla})
            Transliteration : {letter.Transliteration}
            Makhraj type    : {letter.MakhrajType}
            Makhraj desc    : {letter.MakhrajDescription}
            Makhraj (Bangla): {letter.MakhrajDescriptionBn}

            {transcriptLine}
            """);

        var response = await chat.GetChatMessageContentAsync(history, cancellationToken: ct);
        var json     = response.Content
            ?? throw new InvalidOperationException("Empty response from Azure AI.");

        _logger.LogDebug("AI evaluation JSON for '{Letter}': {Json}", letter.Letter, json);

        return ParseResponse(json, letter, transcript);
    }

    private static LetterPronunciationResultDto ParseResponse(
        string          json,
        ArabicLetterDto letter,
        string          transcript)
    {
        var cleaned = json.Trim();
        if (cleaned.StartsWith("```"))
            cleaned = cleaned
                .Replace("```json", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("```",     string.Empty)
                .Trim();

        using var doc  = JsonDocument.Parse(cleaned);
        var       root = doc.RootElement;

        var pronunciationScore = root.GetProperty("pronunciationScore").GetDouble();
        var accuracyScore      = root.GetProperty("accuracyScore").GetDouble();
        var isCorrect          = root.GetProperty("isCorrect").GetBoolean();
        var feedback           = root.GetProperty("feedback").GetString()    ?? string.Empty;
        var feedbackBn         = root.GetProperty("feedbackBn").GetString()  ?? string.Empty;
        var makhrajHint        = root.TryGetProperty("makhrajHint", out var mh)
                                     ? mh.GetString() ?? letter.MakhrajDescriptionBn
                                     : letter.MakhrajDescriptionBn;

        var displayText = transcript == "[SPEECH_DETECTED_BUT_UNCLEAR]" ? "" : transcript;

        return new LetterPronunciationResultDto(
            IsCorrect:      isCorrect,
            Score:          pronunciationScore,
            AccuracyScore:  accuracyScore,
            RecognizedText: displayText,
            Feedback:       feedback,
            FeedbackBn:     feedbackBn,
            MakhrajHint:    makhrajHint);
    }
}
