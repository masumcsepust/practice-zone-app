namespace backend.Application.DTOs;

public record SurahDto(
    int    Id,
    int    SurahNumber,
    string NameArabic,
    string NameEnglish,
    string NameBangla,
    int    TotalAyahs);
