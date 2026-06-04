using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class SurahRepository : ISurahRepository
{
    private readonly AppDbContext _ctx;

    public SurahRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IReadOnlyList<Surah>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Surahs.OrderBy(s => s.SurahNumber).ToListAsync(ct);

    public async Task<Surah?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _ctx.Surahs.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<Surah?> GetBySurahNumberAsync(int surahNumber, CancellationToken ct = default)
        => await _ctx.Surahs.FirstOrDefaultAsync(s => s.SurahNumber == surahNumber, ct);
}
