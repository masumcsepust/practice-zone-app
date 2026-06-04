namespace backend.Domain.Entities;

/// <summary>
/// A diacritical-mark category: one of the 5 Harakat or 3 Tanween types.
/// e.g. Fatha (◌َ), Kasra (◌ِ), Damma (◌ُ), Sukun (◌ْ), Shadda (◌ّ)
///      TanweenFath (◌ً), TanweenKasr (◌ٍ), TanweenDamm (◌ٌ)
/// </summary>
public class HarakatCategory
{
    public int    Id             { get; set; }
    public string Name           { get; set; } = string.Empty; // "Fatha"
    public string ArabicName     { get; set; } = string.Empty; // "فَتْحَة"
    public string BanglaName     { get; set; } = string.Empty; // "ফাতহা"
    public string Symbol         { get; set; } = string.Empty; // combining char ◌َ
    public string Type           { get; set; } = string.Empty; // "Harakat" | "Tanween"
    public string Description    { get; set; } = string.Empty;
    public string DescriptionBn  { get; set; } = string.Empty;
    public int    DisplayOrder   { get; set; }

    public ICollection<LetterHarakatItem> Items { get; set; } = [];
}
