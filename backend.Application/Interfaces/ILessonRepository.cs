using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface ILessonRepository
{
    Task<TanweenLessonData?> GetTanweenLessonAsync(int letterOrder, string baseUrl, CancellationToken ct = default);
}
