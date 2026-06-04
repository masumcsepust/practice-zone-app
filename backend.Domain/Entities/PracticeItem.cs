namespace backend.Domain.Entities;

public class PracticeItem
{
    public Guid  Id                    { get; set; }
    public Guid  LessonId              { get; set; }
    public Guid  TargetSyllableId      { get; set; }
    public Guid? CompareWithSyllableId { get; set; }
    public LocalizedText Instruction   { get; set; } = null!;
    public LocalizedText SuccessTip    { get; set; } = null!;

    public Lesson        Lesson              { get; set; } = null!;
    public SyllableSound TargetSyllable      { get; set; } = null!;
    public SyllableSound? CompareWithSyllable { get; set; }
}
