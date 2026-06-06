namespace backend.Application.DTOs;

public record AyahDto(
    int    Id,
    int    SurahId,
    int    AyahNumber,
    string ArabicText,
    string EnglishTranslation,
    string BanglaTranslation,
    string Transliteration,
    int?   Page,
    int?   Juz,
    int?   Manzil,
    int?   Ruku,
    int?   HizbQuarter,
    bool?  Sajda,
    string AudioUrl);
