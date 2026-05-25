namespace backend.Application.DTOs;

public record WordPronunciationDto(
    string Word,
    double AccuracyScore,
    bool IsCorrect,
    string ErrorType,
    IReadOnlyList<PhonemeDto> Phonemes);
