namespace backend.Domain.Entities;

public class AudioRecord
{
    public int Id { get; set; }
    public int StreamingSessionId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTime RecordedAt { get; set; }
}
