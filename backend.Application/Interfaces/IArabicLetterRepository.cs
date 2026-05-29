using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface IArabicLetterRepository
{
    /// <summary>Returns all letters — kept for internal use (WebSocket handler, forms, etc.).</summary>
    Task<IReadOnlyList<ArabicLetter>>  GetAllAsync(CancellationToken ct = default);

    /// <summary>Returns a single page of letters ordered by Order.</summary>
    Task<PagedResult<ArabicLetter>>    GetPagedAsync(int page, int pageSize, CancellationToken ct = default);

    Task<ArabicLetter?>                GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Returns positional forms for every letter, ordered by Order.</summary>
    Task<IReadOnlyList<LetterFormsDto>> GetAllFormsAsync(CancellationToken ct = default);

    /// <summary>Returns positional forms for a single letter, or null if not found.</summary>
    Task<LetterFormsDto?> GetFormsByIdAsync(int id, CancellationToken ct = default);
}
