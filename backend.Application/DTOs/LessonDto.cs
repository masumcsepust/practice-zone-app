namespace backend.Application.DTOs;

public record LessonCategoryDto(
    int    Id,
    string Name,
    int    ItemCount,
    string TitleEn,
    string TitleBn,
    string DescriptionEn,
    string DescriptionBn
);

public record LessonItemDto(
    int    Id,
    int    CategoryId,
    string ArabicText,
    string SoundText,
    string AudioUrl,
    int    OrderNo,
    string DescriptionEn,
    string DescriptionBn
);
