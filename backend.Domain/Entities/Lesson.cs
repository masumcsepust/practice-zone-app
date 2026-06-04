namespace backend.Domain.Entities;

public class Lesson
{
    public Guid Id            { get; set; }
    public LocalizedText Title { get; set; } = null!;
    public int  SequenceOrder  { get; set; }

    public ICollection<PracticeItem> PracticeItems { get; set; } = [];
}
