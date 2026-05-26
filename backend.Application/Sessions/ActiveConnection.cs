using System.Net.WebSockets;
using System.Text;

namespace backend.Application.Sessions;

public class ActiveConnection
{
    public string ConnectionId { get; init; } = string.Empty;
    public int UserId { get; init; }
    public WebSocket Socket { get; init; } = null!;
    public DateTime StartedAt { get; init; } = DateTime.UtcNow;

    private readonly SemaphoreSlim _sendLock = new(1, 1);

    public async Task SendTextAsync(string message, CancellationToken ct = default)
    {
        if (Socket.State != WebSocketState.Open) return;

        await _sendLock.WaitAsync(ct);
        try
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await Socket.SendAsync(bytes, WebSocketMessageType.Text, true, ct);
        }
        catch (WebSocketException)
        {
            // Socket closed between state-check and send — caller doesn't need to know
        }
        finally
        {
            _sendLock.Release();
        }
    }
}
