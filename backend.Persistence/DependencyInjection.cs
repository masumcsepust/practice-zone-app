using backend.Application.Interfaces;
using backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAyahRepository, AyahRepository>();
        services.AddScoped<ISurahRepository, SurahRepository>();
        services.AddScoped<IArabicLetterRepository, ArabicLetterRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<ITajweedProgressRepository, TajweedProgressRepository>();
        services.AddScoped<ILessonPracticeRepository, LessonPracticeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecitationSessionRepository, RecitationSessionRepository>();
        services.AddScoped<StreamingSessionRepository>();
        services.AddScoped<PronunciationRepository>();

        return services;
    }
}
