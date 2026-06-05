using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class SyllableSoundRepository : ISyllableSoundRepository
{
    private readonly AppDbContext _db;

    public SyllableSoundRepository(AppDbContext db) => _db = db;

    private IQueryable<SyllableSound> WithIncludes()
        => _db.SyllableSounds
              .Include(s => s.Letter)
              .Include(s => s.Sign);

    public async Task<IReadOnlyList<SyllableSound>> GetAllAsync(CancellationToken ct = default)
        => await WithIncludes()
               .OrderBy(s => s.Letter.SequenceOrder)
               .ThenBy(s => s.Sign.SignGroup)
               .ToListAsync(ct);

    public async Task<PagedResult<SyllableSound>> GetPagedAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        page     = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var total = await _db.SyllableSounds.CountAsync(ct);
        var items = await WithIncludes()
            .OrderBy(s => s.Letter.SequenceOrder)
            .ThenBy(s => s.Sign.SignGroup)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<SyllableSound>(items, page, pageSize, total);
    }

    public async Task<SyllableSound?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await WithIncludes().FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<SyllableSound> CreateAsync(SyllableSound entity, CancellationToken ct = default)
    {
        entity.Id = Guid.NewGuid();
        _db.SyllableSounds.Add(entity);
        await _db.SaveChangesAsync(ct);
        return (await WithIncludes().FirstOrDefaultAsync(s => s.Id == entity.Id, ct))!;
    }

    public async Task<SyllableSound?> UpdateAsync(
        Guid id, UpdateSyllableSoundDto dto, CancellationToken ct = default)
    {
        var entity = await _db.SyllableSounds.FindAsync([id], ct);
        if (entity is null) return null;

        entity.CombinedCharacter      = dto.CombinedCharacter;
        entity.Transliteration        = new Domain.Entities.LocalizedText
        {
            En = dto.TransliterationEn,
            Bn = dto.TransliterationBn
        };
        entity.AudioUrl = dto.AudioUrl;

        await _db.SaveChangesAsync(ct);
        return await WithIncludes().FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SyllableSounds.FindAsync([id], ct);
        if (entity is null) return false;

        _db.SyllableSounds.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
