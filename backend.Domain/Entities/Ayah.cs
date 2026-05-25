namespace backend.Domain.Entities;

public class Ayah
{
    public int Id { get; set; }
    public int SurahId { get; set; }
    public Surah Surah { get; set; } = null!;
    public int AyahNumber { get; set; }
    public string ArabicText { get; set; } = string.Empty;
    public string NormalizedArabicText { get; set; } = string.Empty;
    public string EnglishTranslation { get; set; } = string.Empty;
    public string BanglaTranslation { get; set; } = string.Empty;
    public string Transliteration { get; set; } = string.Empty;
}
