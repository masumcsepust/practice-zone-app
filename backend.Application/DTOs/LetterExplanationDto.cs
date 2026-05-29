namespace backend.Application.DTOs;

/// <summary>
/// AI-generated detailed explanation of an Arabic letter produced by Semantic Kernel + Azure OpenAI.
/// Returned by GET /api/arabic-letters/{id}/explanation.
/// </summary>
public record LetterExplanationDto(
    int    Id,
    string Letter,
    string NameEnglish,

    // English sections
    string MakhrajExplanationEn,
    string SifaatExplanationEn,
    string PositionalFormsExplanationEn,
    string PracticeTipsEn,

    // Bangla sections
    string MakhrajExplanationBn,
    string SifaatExplanationBn,
    string PositionalFormsExplanationBn,
    string PracticeTipsBn
);
