namespace backend.Domain.Entities;

public class StreamingSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ConnectionId { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string Status { get; set; } = string.Empty;   // Active | Completed | Disconnected
}
