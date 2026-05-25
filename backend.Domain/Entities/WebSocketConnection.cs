namespace backend.Domain.Entities;

public class WebSocketConnection
{
    public int Id { get; set; }
    public string ConnectionId { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
    public DateTime? DisconnectedAt { get; set; }
}
