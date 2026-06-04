using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class AyahRepository : IAyahRepository
{
    private readonly AppDbContext _ctx;

    public AyahRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Ayah?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _ctx.Ayahs.Include(a => a.Surah).FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<Ayah?> GetBySurahAndAyahAsync(int surahId, int ayahNumber, CancellationToken ct = default)
        => await _ctx.Ayahs.Include(a => a.Surah)
               .FirstOrDefaultAsync(a => a.SurahId == surahId && a.AyahNumber == ayahNumber, ct);

    public async Task<IReadOnlyList<Ayah>> GetBySurahIdAsync(int surahId, CancellationToken ct = default)
        => await _ctx.Ayahs
               .Where(a => a.SurahId == surahId)
               .OrderBy(a => a.AyahNumber)
               .ToListAsync(ct);

    public async Task<IReadOnlyList<Ayah>> GetByPageAsync(int page, CancellationToken ct = default)
        => await _ctx.Ayahs
               .Include(a => a.Surah)
               .Where(a => a.Page == page)
               .OrderBy(a => a.SurahId).ThenBy(a => a.AyahNumber)
               .ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<Ayah> ayahs, CancellationToken ct = default)
    {
        var list = ayahs.ToList();
        var ids  = list.Select(a => a.Id).ToList();
        var existingIds = await _ctx.Ayahs
            .Where(a => ids.Contains(a.Id))
            .Select(a => a.Id)
            .ToHashSetAsync(ct);

        var toInsert = list.Where(a => !existingIds.Contains(a.Id)).ToList();
        if (toInsert.Count > 0)
        {
            await _ctx.Ayahs.AddRangeAsync(toInsert, ct);
            await _ctx.SaveChangesAsync(ct);
        }
    }
}
