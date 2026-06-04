namespace backend.Domain.Entities;

public class UserXpLog
{
    public Guid     Id         { get; set; }
    public Guid     UserId     { get; set; }
    public Guid     LessonId   { get; set; }
    public int      XpEarned   { get; set; }
    public string   ActionType { get; set; } = string.Empty;
    public DateTime EarnedAt   { get; set; }

    public User        User   { get; set; } = null!;
    public Lesson      Lesson { get; set; } = null!;
}
