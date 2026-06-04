namespace backend.Domain.Entities;

public class TajweedPracticeRecord
{
    public int      Id            { get; set; }
    public Guid     LetterId      { get; set; }
    /// <summary>tanween | sukoon_shaddah | word_building</summary>
    public string   Mode          { get; set; } = string.Empty;
    public double   Score         { get; set; }
    public double   AccuracyScore { get; set; }
    public bool     IsCorrect     { get; set; }
    public DateTime PracticedAt   { get; set; } = DateTime.UtcNow;

    public ArabicLetter Letter { get; set; } = null!;
}
