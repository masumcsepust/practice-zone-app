namespace backend.Domain.Entities;

/// <summary>
/// One row = one letter + one diacritical-mark category.
/// Stores the resulting sound label and an example Quran word.
/// e.g. Letter=ب, Category=Fatha → Sound="Ba", ExampleWord="بَيْت"
/// </summary>
public class LetterHarakatItem
{
    public int    Id          { get; set; }
    public Guid   LetterId    { get; set; }
    public int    CategoryId  { get; set; }
    public string Sound       { get; set; } = string.Empty; // "Ba" / "Ban" / ""
    public string ExampleWord { get; set; } = string.Empty; // Arabic Quran word

    public ArabicLetter    Letter   { get; set; } = null!;
    public HarakatCategory Category { get; set; } = null!;
}
