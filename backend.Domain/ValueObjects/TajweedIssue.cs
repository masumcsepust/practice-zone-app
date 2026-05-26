namespace backend.Domain.ValueObjects;

public class TajweedIssue
{
    public string  RuleType           { get; set; } = string.Empty;
    public string  Word               { get; set; } = string.Empty;
    public string  AffectedLetter     { get; set; } = string.Empty;
    public string  Feedback           { get; set; } = string.Empty;
    public string  Severity           { get; set; } = "medium";   // low | medium | high
    public double? ActualDurationMs   { get; set; }
    public double? ExpectedDurationMs { get; set; }
}
