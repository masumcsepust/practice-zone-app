using backend.Application.Interfaces;
using backend.Infrastructure.AI;
using backend.Infrastructure.Auth;
using backend.Infrastructure.Speech;
using backend.Infrastructure.Storage;
using backend.Infrastructure.Tajweed;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IArabicLetterTtsService, ArabicLetterTtsService>();

        // Semantic Kernel — Azure OpenAI letter explanation (singleton: Kernel is thread-safe)
        services.AddSingleton<ILetterExplanationService, SemanticKernelLetterExplanationService>();

        // Azure OpenAI Vision — letter drawing check (singleton: same Kernel lifetime reason)
        services.AddSingleton<ILetterDrawingService, AzureOpenAIDrawingCheckService>();

        // Phase 1: one-shot file-based speech recognition
        services.AddScoped<ISpeechRecognitionService, AzureSpeechRecognitionService>();
        services.AddScoped<IAudioStorageService, LocalAudioStorageService>();

        // Phase 2: realtime continuous recognition — Singleton to hold recognizer lifetime
        services.AddSingleton<IRealtimeSpeechService, AzureRealtimeSpeechService>();

        // Phase 3: pronunciation assessment — Singleton (same lifetime reason as Phase 2)
        services.AddSingleton<IPronunciationAssessmentService, AzurePronunciationService>();

        // HTTP-based letter pronunciation check (replaces unreliable WS flow)
        services.AddSingleton<ILetterPronunciationService, AzureAILetterPronunciationService>();

        // Phase 4: Tajweed detection — all stateless, safe as singletons
        services.AddSingleton<IMaddDetectionService,     MaddDetectionService>();
        services.AddSingleton<IGhunnahDetectionService,  GhunnahDetectionService>();
        services.AddSingleton<IQalqalahDetectionService, QalqalahDetectionService>();
        services.AddSingleton<IIkhfaDetectionService,    IkhfaDetectionService>();
        services.AddSingleton<IIdghamDetectionService,   IdghamDetectionService>();
        services.AddSingleton<ITajweedEngine,            TajweedEngine>();

        return services;
    }
}
