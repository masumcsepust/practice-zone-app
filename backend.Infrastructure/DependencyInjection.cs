using backend.Application.Interfaces;
using backend.Infrastructure.AI;
using backend.Infrastructure.Auth;
using backend.Infrastructure.Speech;
using backend.Infrastructure.Tajweed;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IArabicLetterTtsService, ArabicLetterTtsService>();
        services.AddSingleton<ILetterDrawingService, AzureOpenAIDrawingCheckService>();
        services.AddSingleton<IPronunciationAssessmentService, AzurePronunciationService>();
        services.AddSingleton<ILetterPronunciationService, AzureAILetterPronunciationService>();

        // Tajweed detection
        services.AddSingleton<IMaddDetectionService,     MaddDetectionService>();
        services.AddSingleton<IGhunnahDetectionService,  GhunnahDetectionService>();
        services.AddSingleton<IQalqalahDetectionService, QalqalahDetectionService>();
        services.AddSingleton<IIkhfaDetectionService,    IkhfaDetectionService>();
        services.AddSingleton<IIdghamDetectionService,   IdghamDetectionService>();
        services.AddSingleton<ITajweedEngine,            TajweedEngine>();

        return services;
    }
}
