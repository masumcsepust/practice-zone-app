using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class ArabicLetterRepository : IArabicLetterRepository
{
    private readonly AppDbContext _db;

    public ArabicLetterRepository(AppDbContext db) => _db = db;

    private IQueryable<ArabicLetter> WithHarakat()
        => _db.ArabicLetters
              .Include(l => l.HarakatItems)
                  .ThenInclude(i => i.Category);

    public async Task<IReadOnlyList<ArabicLetter>> GetAllAsync(CancellationToken ct = default)
        => await WithHarakat().OrderBy(l => l.SequenceOrder).ToListAsync(ct);

    public async Task<PagedResult<ArabicLetter>> GetPagedAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        page     = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var total = await _db.ArabicLetters.CountAsync(ct);
        var items = await WithHarakat()
            .OrderBy(l => l.SequenceOrder)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<ArabicLetter>(items, page, pageSize, total);
    }

    public async Task<ArabicLetter?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await WithHarakat().FirstOrDefaultAsync(l => l.Id == id, ct);

    public async Task<IReadOnlyList<LetterFormsDto>> GetAllFormsAsync(CancellationToken ct = default)
        => await _db.ArabicLetters
            .OrderBy(l => l.SequenceOrder)
            .Select(l => new LetterFormsDto(
                l.Id,
                l.SequenceOrder,
                l.Character,
                l.Name.En,
                l.IsolatedForm,
                l.InitialForm,
                l.MedialForm,
                l.FinalForm,
                l.IsConnector))
            .ToListAsync(ct);

    public async Task<LetterFormsDto?> GetFormsByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.ArabicLetters
            .Where(l => l.Id == id)
            .Select(l => new LetterFormsDto(
                l.Id,
                l.SequenceOrder,
                l.Character,
                l.Name.En,
                l.IsolatedForm,
                l.InitialForm,
                l.MedialForm,
                l.FinalForm,
                l.IsConnector))
            .FirstOrDefaultAsync(ct);
}
