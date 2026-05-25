using System.Collections.Concurrent;
using System.Net.WebSockets;
using backend.Application.Interfaces;
using backend.Application.Sessions;

namespace backend.Application.Services;

public class WebSocketSessionManager : IWebSocketSessionManager
{
    private readonly ConcurrentDictionary<string, ActiveConnection> _connections = new();

    public string AddConnection(WebSocket socket, int userId)
    {
        var connectionId = Guid.NewGuid().ToString();

        _connections[connectionId] = new ActiveConnection
        {
            ConnectionId = connectionId,
            UserId       = userId,
            Socket       = socket
        };

        return connectionId;
    }

    public ActiveConnection? GetConnection(string connectionId)
        => _connections.TryGetValue(connectionId, out var conn) ? conn : null;

    public void RemoveConnection(string connectionId)
        => _connections.TryRemove(connectionId, out _);
}
