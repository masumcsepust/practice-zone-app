using backend.Application.Interfaces;
using backend.Infrastructure.Speech;
using backend.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Phase 1: one-shot file-based speech recognition
        services.AddScoped<ISpeechRecognitionService, AzureSpeechRecognitionService>();
        services.AddScoped<IAudioStorageService, LocalAudioStorageService>();

        // Phase 2: realtime continuous recognition — Singleton to hold recognizer lifetime
        services.AddSingleton<IRealtimeSpeechService, AzureRealtimeSpeechService>();

        return services;
    }
}
