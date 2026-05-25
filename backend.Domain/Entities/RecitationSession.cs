namespace backend.Domain.Entities;

public class RecitationSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AyahId { get; set; }
    public Ayah Ayah { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string RecognizedText { get; set; } = string.Empty;
    public string NormalizedRecognizedText { get; set; } = string.Empty;
    public int OverallScore { get; set; }
    public string AudioFilePath { get; set; } = string.Empty;
}
