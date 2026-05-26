using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface IAyahRepository
{
    Task<Ayah?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Ayah?> GetBySurahAndAyahAsync(int surahId, int ayahNumber, CancellationToken ct = default);
    Task<IReadOnlyList<Ayah>> GetBySurahIdAsync(int surahId, CancellationToken ct = default);
}
