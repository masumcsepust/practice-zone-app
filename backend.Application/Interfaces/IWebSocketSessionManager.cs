using System.Net.WebSockets;
using backend.Application.Sessions;

namespace backend.Application.Interfaces;

public interface IWebSocketSessionManager
{
    string AddConnection(WebSocket socket, int userId);
    ActiveConnection? GetConnection(string connectionId);
    void RemoveConnection(string connectionId);
}
