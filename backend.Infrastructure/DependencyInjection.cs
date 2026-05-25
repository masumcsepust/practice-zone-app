using backend.Application.Interfaces;
using backend.Infrastructure.Speech;
using backend.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISpeechRecognitionService, AzureSpeechRecognitionService>();
        services.AddScoped<IAudioStorageService, LocalAudioStorageService>();
        return services;
    }
}
