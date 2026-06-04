using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface IArabicLetterRepository
{
    Task<IReadOnlyList<ArabicLetter>>   GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<ArabicLetter>>     GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<ArabicLetter?>                 GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<LetterFormsDto>> GetAllFormsAsync(CancellationToken ct = default);
    Task<LetterFormsDto?>               GetFormsByIdAsync(Guid id, CancellationToken ct = default);
}
