using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class ArabicLetterRepository : IArabicLetterRepository
{
    private readonly AppDbContext _db;

    public ArabicLetterRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ArabicLetter>> GetAllAsync(CancellationToken ct = default)
        => await _db.ArabicLetters.OrderBy(l => l.Order).ToListAsync(ct);

    public async Task<PagedResult<ArabicLetter>> GetPagedAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        page     = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var total = await _db.ArabicLetters.CountAsync(ct);
        var items = await _db.ArabicLetters
            .OrderBy(l => l.Order)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<ArabicLetter>(items, page, pageSize, total);
    }

    public async Task<ArabicLetter?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.ArabicLetters.FirstOrDefaultAsync(l => l.Id == id, ct);

    public async Task<IReadOnlyList<LetterFormsDto>> GetAllFormsAsync(CancellationToken ct = default)
        => await _db.ArabicLetters
            .OrderBy(l => l.Order)
            .Select(l => new LetterFormsDto(
                l.Id,
                l.Order,
                l.Letter,
                l.NameEnglish,
                l.IsolatedForm,
                l.InitialForm,
                l.MedialForm,
                l.FinalForm,
                l.IsConnector))
            .ToListAsync(ct);

    public async Task<LetterFormsDto?> GetFormsByIdAsync(int id, CancellationToken ct = default)
        => await _db.ArabicLetters
            .Where(l => l.Id == id)
            .Select(l => new LetterFormsDto(
                l.Id,
                l.Order,
                l.Letter,
                l.NameEnglish,
                l.IsolatedForm,
                l.InitialForm,
                l.MedialForm,
                l.FinalForm,
                l.IsConnector))
            .FirstOrDefaultAsync(ct);
}
