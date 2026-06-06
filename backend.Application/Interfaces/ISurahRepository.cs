using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface ISurahRepository
{
    Task<IReadOnlyList<Surah>> GetAllAsync(CancellationToken ct = default);
    Task<Surah?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Surah?> GetBySurahNumberAsync(int surahNumber, CancellationToken ct = default);
    Task UpsertAsync(Surah surah, CancellationToken ct = default);
}
