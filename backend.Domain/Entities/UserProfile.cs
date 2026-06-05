namespace backend.Domain.Entities;

public class UserProfile
{
    public Guid     Id                 { get; set; }
    public Guid     UserId             { get; set; }
    public string   DisplayName        { get; set; } = string.Empty;
    public string   AvatarUrl          { get; set; } = string.Empty;
    public string   Username           { get; set; } = string.Empty;
    public int      TotalXp            { get; set; }
    public int      CurrentStreak      { get; set; }
    public int      TotalLessons       { get; set; }
    public double   CorrectAnswerRate  { get; set; }
    public DateTime CreatedAt          { get; set; }
    public DateTime UpdatedAt          { get; set; }

    public User                   User    { get; set; } = null!;
    public ICollection<UserXpLog> XpLogs  { get; set; } = [];
}
