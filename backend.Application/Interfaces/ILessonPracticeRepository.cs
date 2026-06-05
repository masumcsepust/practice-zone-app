using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface ILessonPracticeRepository
{
    Task<PracticeSessionData?> GetByIdAsync(Guid lessonId, CancellationToken ct = default);
    Task<IReadOnlyList<PracticeSessionData>> GetAllAsync(CancellationToken ct = default);
    Task<PracticeStepDto?> GetStepAsync(int lessonNum, int stepNum, CancellationToken ct = default);
}
