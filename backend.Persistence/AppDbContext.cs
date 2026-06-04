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
    public DbSet<PronunciationAssessmentResult> PronunciationAssessmentResults => Set<PronunciationAssessmentResult>();
    public DbSet<WordPronunciationResult> WordPronunciationResults => Set<WordPronunciationResult>();
    public DbSet<PhonemeResult> PhonemeResults => Set<PhonemeResult>();
    public DbSet<TajweedRule> TajweedRules => Set<TajweedRule>();
    public DbSet<ArabicLetter>      ArabicLetters      => Set<ArabicLetter>();
    public DbSet<LessonCategory>    LessonCategories   => Set<LessonCategory>();
    public DbSet<LessonItem>        LessonItems        => Set<LessonItem>();
    public DbSet<HarakatCategory>   HarakatCategories  => Set<HarakatCategory>();
    public DbSet<LetterHarakatItem> LetterHarakatItems => Set<LetterHarakatItem>();
    public DbSet<TajweedPracticeRecord> TajweedPracticeRecords => Set<TajweedPracticeRecord>();
    public DbSet<User>           Users           => Set<User>();
    public DbSet<UserProfile>    UserProfiles    => Set<UserProfile>();
    public DbSet<UserXpLog>      UserXpLogs      => Set<UserXpLog>();
    public DbSet<DiacriticSign>  DiacriticSigns  => Set<DiacriticSign>();
    public DbSet<SyllableSound>  SyllableSounds  => Set<SyllableSound>();
    public DbSet<Lesson>         Lessons         => Set<Lesson>();
    public DbSet<PracticeItem>   PracticeItems   => Set<PracticeItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
