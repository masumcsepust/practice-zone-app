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
/// Uses Microsoft Semantic Kernel (Azure AI Inference backend) to generate
/// a rich bilingual (English + Bangla) explanation of an Arabic letter's
/// makhraj, sifaat, and positional forms.
/// </summary>
public sealed class SemanticKernelLetterExplanationService : ILetterExplanationService
{
    private readonly Kernel? _kernel;
    private readonly ILogger<SemanticKernelLetterExplanationService> _logger;

    // ── System prompt ────────────────────────────────────────────────────────
    private const string SystemPrompt = """
        You are an expert Arabic linguistics and Tajweed teacher.
        You will receive structured data about an Arabic letter and must produce a detailed,
        student-friendly explanation in BOTH English and Bangla (বাংলা).

        Respond ONLY with a valid JSON object matching this exact schema (no markdown, no extra text):
        {
          "makhrajExplanationEn": "string",
          "sifaatExplanationEn":  "string",
          "positionalFormsExplanationEn": "string",
          "practiceTipsEn":       "string",
          "makhrajExplanationBn": "string",
          "sifaatExplanationBn":  "string",
          "positionalFormsExplanationBn": "string",
          "practiceTipsBn":       "string"
        }

        Guidelines:
        - makhrajExplanation: describe exactly where and how the letter is articulated in the mouth/throat (2-4 sentences).
        - sifaatExplanation:  explain each sifah (characteristic) listed and how it affects pronunciation (2-4 sentences).
        - positionalFormsExplanation: explain how the letter's shape changes at the start, middle, end, and isolation (1-3 sentences).
        - practiceTips: give 2-3 practical beginner tips for correctly pronouncing this letter.
        - Keep Bangla text natural and clear for a Bangladeshi learner.
        """;

    public SemanticKernelLetterExplanationService(
        IConfiguration config,
        ILogger<SemanticKernelLetterExplanationService> logger)
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
            _logger.LogWarning("Azure:AI:Endpoint/ApiKey not configured — letter explanation AI disabled.");
            _kernel = null;
        }
    }

    public async Task<LetterExplanationDto> ExplainAsync(ArabicLetterDto letter, CancellationToken ct = default)
    {
        _logger.LogInformation("Generating AI explanation for letter '{Letter}' ({Name})",
            letter.Letter, letter.NameEnglish);

        if (_kernel is null)
            throw new InvalidOperationException("Letter explanation AI is not configured (missing Azure:AI credentials).");

        var chat = _kernel.GetRequiredService<IChatCompletionService>();

        var history = new ChatHistory(SystemPrompt);
        history.AddUserMessage(BuildUserPrompt(letter));

        var response = await chat.GetChatMessageContentAsync(history, cancellationToken: ct);
        var json     = response.Content ?? throw new InvalidOperationException("Empty response from Azure OpenAI.");

        _logger.LogDebug("Raw AI response for '{Letter}': {Json}", letter.Letter, json);

        return ParseResponse(json, letter);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string BuildUserPrompt(ArabicLetterDto l) => $"""
        Letter:        {l.Letter}  ({l.NameEnglish} / {l.NameArabic} / {l.NameBangla})
        Transliteration: {l.Transliteration}
        Makhraj type:  {l.MakhrajType}
        Makhraj desc:  {l.MakhrajDescription}
        Sifaat:        {string.Join(", ", l.Sifaat)}
        IsConnector:   {l.IsConnector}
        Isolated form: {l.IsolatedForm}
        Initial form:  {l.InitialForm}
        Medial form:   {l.MedialForm}
        Final form:    {l.FinalForm}
        Example word:  {l.ExampleWordArabic} ({l.ExampleWord} / {l.ExampleWordBn})
        """;

    private static LetterExplanationDto ParseResponse(string json, ArabicLetterDto letter)
    {
        // Strip markdown fences if the model wrapped the JSON
        var cleaned = json.Trim();
        if (cleaned.StartsWith("```"))
        {
            cleaned = cleaned
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();
        }

        using var doc = JsonDocument.Parse(cleaned);
        var root = doc.RootElement;

        return new LetterExplanationDto(
            Id:                          letter.Id,
            Letter:                      letter.Letter,
            NameEnglish:                 letter.NameEnglish,
            MakhrajExplanationEn:        root.GetProperty("makhrajExplanationEn").GetString()     ?? "",
            SifaatExplanationEn:         root.GetProperty("sifaatExplanationEn").GetString()      ?? "",
            PositionalFormsExplanationEn:root.GetProperty("positionalFormsExplanationEn").GetString() ?? "",
            PracticeTipsEn:              root.GetProperty("practiceTipsEn").GetString()           ?? "",
            MakhrajExplanationBn:        root.GetProperty("makhrajExplanationBn").GetString()     ?? "",
            SifaatExplanationBn:         root.GetProperty("sifaatExplanationBn").GetString()      ?? "",
            PositionalFormsExplanationBn:root.GetProperty("positionalFormsExplanationBn").GetString() ?? "",
            PracticeTipsBn:              root.GetProperty("practiceTipsBn").GetString()           ?? ""
        );
    }
}
