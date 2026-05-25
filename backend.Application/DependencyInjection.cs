using backend.Application.Interfaces;
using backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IArabicTextNormalizer, ArabicTextNormalizer>();
        services.AddScoped<IAyahComparisonService, AyahComparisonService>();
        services.AddScoped<IRecitationService, RecitationService>();
        return services;
    }
}
