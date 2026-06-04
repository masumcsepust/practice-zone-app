namespace backend.Domain.Entities;

public class LessonItem
{
    public int    Id            { get; set; }
    public int    CategoryId    { get; set; }
    public string ArabicText    { get; set; } = string.Empty;
    public string SoundText     { get; set; } = string.Empty;
    public string AudioUrl      { get; set; } = string.Empty;
    public int    OrderNo       { get; set; }
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionBn { get; set; } = string.Empty;

    public LessonCategory Category { get; set; } = null!;
}
