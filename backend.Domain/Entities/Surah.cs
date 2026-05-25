namespace backend.Domain.Entities;

public class Surah
{
    public int Id { get; set; }
    public int SurahNumber { get; set; }
    public string NameArabic { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string NameBangla { get; set; } = string.Empty;
    public int TotalAyahs { get; set; }
}
