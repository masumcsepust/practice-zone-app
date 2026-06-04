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

    public int? Page { get; set; }
    public int? Juz { get; set; }
    public int? Manzil { get; set; }
    public int? Ruku { get; set; }
    public int? HizbQuarter { get; set; }
    public bool? Sajda { get; set; }
}
