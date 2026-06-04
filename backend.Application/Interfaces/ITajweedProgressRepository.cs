using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface ITajweedProgressRepository
{
    Task SaveAsync(TajweedPracticeRecord record, CancellationToken ct = default);
    Task<IReadOnlyList<TajweedLetterProgress>> GetTableAsync(CancellationToken ct = default);
}
