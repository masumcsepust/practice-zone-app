using backend.Application.Interfaces;
using backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Singleton: holds live WebSocket connections across the app lifetime
        services.AddSingleton<IWebSocketSessionManager, WebSocketSessionManager>();

        return services;
    }
}
