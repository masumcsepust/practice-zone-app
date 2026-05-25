using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface IRecitationSessionRepository
{
    Task AddAsync(RecitationSession session, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
