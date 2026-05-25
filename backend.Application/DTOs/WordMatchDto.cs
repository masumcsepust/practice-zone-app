namespace backend.Application.DTOs;

public class WordMatchDto
{
    public string ExpectedWord { get; init; } = string.Empty;
    public string RecognizedWord { get; init; } = string.Empty;
    public bool IsCorrect { get; init; }
    public int SimilarityScore { get; init; }
}
