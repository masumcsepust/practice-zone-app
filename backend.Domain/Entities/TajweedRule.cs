namespace backend.Domain.Entities;

public class TajweedRule
{
    public int    Id          { get; set; }
    public string RuleType    { get; set; } = string.Empty;  // Madd | Ghunnah | Qalqalah | Ikhfa | Idgham
    public string ArabicName  { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
