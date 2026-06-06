using backend.Application.Interfaces;
using backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IWebSocketSessionManager, WebSocketSessionManager>();
        services.AddSingleton<IPhonemeAnalysisService, PhonemeAnalysisService>();

        return services;
    }
}
