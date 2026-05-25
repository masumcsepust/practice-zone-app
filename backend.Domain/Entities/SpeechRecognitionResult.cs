namespace backend.Domain.Entities;

public class SpeechRecognitionResult
{
    public int Id { get; set; }
    public int StreamingSessionId { get; set; }
    public StreamingSession StreamingSession { get; set; } = null!;
    public string RecognizedText { get; set; } = string.Empty;
    public string ResultType { get; set; } = string.Empty;   // Partial | Final
    public DateTime RecognizedAt { get; set; }
}
