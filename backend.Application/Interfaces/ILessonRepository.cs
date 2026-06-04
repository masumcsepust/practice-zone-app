using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface ILessonRepository
{
    Task<IReadOnlyList<LessonCategoryDto>> GetCategoriesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LessonItemDto>>     GetItemsAsync(int categoryId, CancellationToken ct = default);
    Task<LessonItemDto?>                   GetItemByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Structured tanween lesson for letter at position 1–28.</summary>
    Task<TanweenLessonData?> GetTanweenLessonAsync(int letterOrder, string baseUrl, CancellationToken ct = default);

    /// <summary>Structured harakat lesson for letter at position 1–28.</summary>
    Task<HarakatLessonData?> GetHarakatLessonAsync(int letterOrder, string baseUrl, CancellationToken ct = default);
}
