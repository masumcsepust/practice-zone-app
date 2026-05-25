namespace backend.Domain.Entities;

public class PronunciationAssessmentResult
{
    public int Id { get; set; }
    public int StreamingSessionId { get; set; }
    public int AyahId { get; set; }
    public string RecognizedText { get; set; } = string.Empty;
    public double AccuracyScore { get; set; }
    public double FluencyScore { get; set; }
    public double CompletenessScore { get; set; }
    public double PronunciationScore { get; set; }
    public DateTime CreatedAt { get; set; }

    public StreamingSession StreamingSession { get; set; } = null!;
    public Ayah Ayah { get; set; } = null!;
    public ICollection<WordPronunciationResult> Words { get; set; } = [];
}
