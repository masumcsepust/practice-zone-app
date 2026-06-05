using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface ISyllableSoundRepository
{
    Task<IReadOnlyList<SyllableSound>> GetAllAsync(CancellationToken ct = default);
    Task<PagedResult<SyllableSound>>   GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<SyllableSound?>               GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SyllableSound>                CreateAsync(SyllableSound entity, CancellationToken ct = default);
    Task<SyllableSound?>               UpdateAsync(Guid id, UpdateSyllableSoundDto dto, CancellationToken ct = default);
    Task<bool>                         DeleteAsync(Guid id, CancellationToken ct = default);
}
