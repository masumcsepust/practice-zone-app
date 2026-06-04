using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class TajweedProgressRepository(AppDbContext db) : ITajweedProgressRepository
{
    public async Task SaveAsync(TajweedPracticeRecord record, CancellationToken ct = default)
    {
        db.TajweedPracticeRecords.Add(record);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<TajweedLetterProgress>> GetTableAsync(CancellationToken ct = default)
    {
        var records = await db.TajweedPracticeRecords
            .Include(r => r.Letter)
            .ToListAsync(ct);

        return records
            .GroupBy(r => r.LetterId)
            .Select(g =>
            {
                var letter = g.First().Letter;
                return new TajweedLetterProgress(
                    LetterId:      letter.Id,
                    Letter:        letter.Character,
                    NameBangla:    letter.Name.Bn,
                    Harakat:       Stat(g, "harakat"),
                    Tanween:       Stat(g, "tanween"),
                    SukoonShaddah: Stat(g, "sukoon_shaddah"),
                    WordBuilding:  Stat(g, "word_building")
                );
            })
            .OrderBy(p => p.LetterId)
            .ToList()
            .AsReadOnly();
    }

    private static TajweedModeStat? Stat(IEnumerable<TajweedPracticeRecord> records, string mode)
    {
        var subset = records.Where(r => r.Mode == mode).ToList();
        if (subset.Count == 0) return null;
        return new TajweedModeStat(
            BestScore:   subset.Max(r => r.Score),
            Attempts:    subset.Count,
            LastCorrect: subset.OrderByDescending(r => r.PracticedAt).First().IsCorrect
        );
    }
}
