namespace backend.Application.DTOs;

public record PhonemeDto(
    string Phoneme,
    double AccuracyScore,
    long Duration,
    bool IsWeak);
