namespace backend.Domain.Entities;

public class WordPronunciationResult
{
    public int Id { get; set; }
    public int PronunciationAssessmentResultId { get; set; }
    public string Word { get; set; } = string.Empty;
    public double AccuracyScore { get; set; }
    public string ErrorType { get; set; } = "None";  // None | Omission | Insertion | Mispronunciation
    public bool IsCorrect { get; set; }
    public long Offset { get; set; }    // 100ns ticks from Azure
    public long Duration { get; set; }  // 100ns ticks from Azure

    public PronunciationAssessmentResult Assessment { get; set; } = null!;
    public ICollection<PhonemeResult> Phonemes { get; set; } = [];
}
