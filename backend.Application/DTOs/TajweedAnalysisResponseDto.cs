namespace backend.Application.DTOs;

public record TajweedAnalysisResponseDto
{
    public string Type              { get; init; } = "tajweed";
    public string RecognizedText    { get; init; } = string.Empty;
    public double PronunciationScore { get; init; }
    public double AccuracyScore     { get; init; }
    public double FluencyScore      { get; init; }
    public double CompletenessScore { get; init; }
    public double TajweedScore      { get; init; }
    public IReadOnlyList<WordPronunciationDto> Words        { get; init; } = [];
    public IReadOnlyList<PhonemeDto>           WeakPhonemes { get; init; } = [];
    public IReadOnlyList<TajweedIssueDto>      TajweedIssues { get; init; } = [];
}
