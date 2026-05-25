namespace backend.Domain.Entities;

public class PhonemeResult
{
    public int Id { get; set; }
    public int WordPronunciationResultId { get; set; }
    public string Phoneme { get; set; } = string.Empty;  // IPA symbol from Azure
    public double AccuracyScore { get; set; }
    public long Duration { get; set; }   // 100ns ticks from Azure
    public bool IsWeak { get; set; }     // AccuracyScore < 70

    public WordPronunciationResult Word { get; set; } = null!;
}
