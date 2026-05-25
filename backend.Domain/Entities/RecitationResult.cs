namespace backend.Domain.Entities;

public class RecitationResult
{
    public int OverallScore { get; init; }
    public string RecognizedText { get; init; } = string.Empty;
    public IReadOnlyList<WordMatchResult> WordMatches { get; init; } = [];
}
