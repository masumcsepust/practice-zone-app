using System.Reflection;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Surah> Surahs => Set<Surah>();
    public DbSet<Ayah> Ayahs => Set<Ayah>();
    public DbSet<RecitationSession> RecitationSessions => Set<RecitationSession>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
