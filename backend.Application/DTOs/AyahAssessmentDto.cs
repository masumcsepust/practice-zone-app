namespace backend.Application.DTOs;

public record AyahAssessmentDto(
    int    OverallTajweed,
    int    LettersPct,
    float  PacePct,
    string RecognizedText,
    IReadOnlyList<AyahNoteDto> Notes);

public record AyahNoteDto(
    string  Kind,               // QALQALAH | MADD | GHUNNAH | IDGHAAM | IKHFAA | POSITIVE
    string  Arabic,
    string  Title,
    string  Tip,
    bool    IsPositive,
    string? ReferenceAudioUrl);
