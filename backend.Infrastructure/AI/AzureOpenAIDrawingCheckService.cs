using System.Text.Json;
using Azure;
using Azure.AI.Inference;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace backend.Infrastructure.AI;

/// <summary>
/// Sends the user's drawn-letter image to a vision-capable model via Azure AI Inference
/// and returns a structured bilingual (English + Bangla) correction result.
/// </summary>
public sealed class AzureOpenAIDrawingCheckService : ILetterDrawingService
{
    private readonly Kernel? _kernel;
    private readonly ILogger<AzureOpenAIDrawingCheckService> _logger;

    private const string SystemPrompt = """
        You are an expert Arabic calligraphy teacher evaluating a beginner student's handwritten letter.
        The student drew the letter on a white canvas using a finger/stylus.
        You will receive: the target Arabic letter info, and an image of the student's drawing.

        Evaluate if the drawing resembles the target Arabic letter.
        Be encouraging but honest. Consider:
        - Overall shape similarity to the Arabic letter
        - Correct stroke count and direction
        - Proper proportions
        - Correct dots (nuqta) if required

        Respond ONLY with a valid JSON object (no markdown, no extra text):
        {
          "isCorrect": true/false,
          "score": 0-100,
          "feedback": "concise English feedback (1-2 sentences)",
          "feedbackBn": "same in Bangla (1-2 sentences)",
          "hint": "one specific English improvement tip",
          "hintBn": "same tip in Bangla"
        }

        Score guide:
        - 80-100: Clearly recognizable, good shape
        - 60-79:  Recognizable but needs improvement
        - 40-59:  Partially correct shape
        - 0-39:   Unrecognizable or wrong letter
        """;

    public AzureOpenAIDrawingCheckService(
        IConfiguration config,
        ILogger<AzureOpenAIDrawingCheckService> logger)
    {
        _logger = logger;

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
            _logger.LogWarning("Azure:AI:Endpoint/ApiKey not configured — drawing check AI disabled.");
            _kernel = null;
        }
    }

    public async Task<DrawingCheckResultDto> CheckDrawingAsync(
        ArabicLetterDto   letter,
        string            imageBase64,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Checking drawing for letter '{Letter}' ({Name})",
            letter.Letter, letter.NameEnglish);

        if (_kernel is null)
            throw new InvalidOperationException("Drawing check AI is not configured (missing Azure:AI credentials).");

        var chat = _kernel.GetRequiredService<IChatCompletionService>();

        var history = new ChatHistory(SystemPrompt);

        // Build a multi-part user message: text context + image
        var userContent = new ChatMessageContentItemCollection
        {
            new TextContent($"""
                Target letter : {letter.Letter}  ({letter.NameEnglish} / {letter.NameArabic} / {letter.NameBangla})
                Required dots : {(letter.Letter is "ب" or "ت" or "ث" or "ن" or "ي" or "ى" ? "yes" : "check letter")}
                Isolated form : {letter.IsolatedForm}
                The student's drawing is attached.
                """),
            new ImageContent(new BinaryData(Convert.FromBase64String(imageBase64)), "image/png")
        };
        history.AddUserMessage(userContent);

        var response = await chat.GetChatMessageContentAsync(history, cancellationToken: ct);
        var json     = response.Content
            ?? throw new InvalidOperationException("Empty response from Azure OpenAI vision.");

        _logger.LogDebug("Drawing check raw response for '{Letter}': {Json}", letter.Letter, json);

        return ParseResponse(json);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static DrawingCheckResultDto ParseResponse(string json)
    {
        var cleaned = json.Trim();
        if (cleaned.StartsWith("```"))
            cleaned = cleaned.Replace("```json", "").Replace("```", "").Trim();

        using var doc  = JsonDocument.Parse(cleaned);
        var       root = doc.RootElement;

        return new DrawingCheckResultDto(
            IsCorrect:  root.GetProperty("isCorrect").GetBoolean(),
            Score:      root.GetProperty("score").GetInt32(),
            Feedback:   root.GetProperty("feedback").GetString()   ?? "",
            FeedbackBn: root.GetProperty("feedbackBn").GetString() ?? "",
            Hint:       root.GetProperty("hint").GetString()       ?? "",
            HintBn:     root.GetProperty("hintBn").GetString()     ?? ""
        );
    }
}
