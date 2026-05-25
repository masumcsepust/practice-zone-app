namespace backend.Application.DTOs;

public record PronunciationResponseDto
{
    public string Type { get; init; } = "pronunciation";
    public string RecognizedText { get; init; } = string.Empty;
    public double PronunciationScore { get; init; }
    public double AccuracyScore { get; init; }
    public double FluencyScore { get; init; }
    public double CompletenessScore { get; init; }
    public IReadOnlyList<WordPronunciationDto> Words { get; init; } = [];
    public IReadOnlyList<PhonemeDto> WeakPhonemes { get; init; } = [];
}
