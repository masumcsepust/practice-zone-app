using System.Reflection;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Surah> Surahs => Set<Surah>();
    public DbSet<Ayah> Ayahs => Set<Ayah>();
    public DbSet<RecitationSession> RecitationSessions => Set<RecitationSession>();
    public DbSet<StreamingSession> StreamingSessions => Set<StreamingSession>();
    public DbSet<SpeechRecognitionResult> SpeechRecognitionResults => Set<SpeechRecognitionResult>();
    public DbSet<AudioRecord> AudioRecords => Set<AudioRecord>();
    public DbSet<WebSocketConnection> WebSocketConnections => Set<WebSocketConnection>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
