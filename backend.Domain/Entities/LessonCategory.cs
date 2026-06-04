namespace backend.Domain.Entities;

public class LessonCategory
{
    public int    Id            { get; set; }
    public string Name          { get; set; } = string.Empty;

    // Concept explanation — shown at the top of each lesson screen
    public string TitleEn       { get; set; } = string.Empty;
    public string TitleBn       { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionBn { get; set; } = string.Empty;

    public ICollection<LessonItem> Items { get; set; } = [];
}
