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
}
