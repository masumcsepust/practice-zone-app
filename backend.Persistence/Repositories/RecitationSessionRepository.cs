using backend.Application.Interfaces;
using backend.Domain.Entities;

namespace backend.Persistence.Repositories;

public class RecitationSessionRepository : IRecitationSessionRepository
{
    private readonly AppDbContext _ctx;

    public RecitationSessionRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task AddAsync(RecitationSession session, CancellationToken ct = default)
        => await _ctx.RecitationSessions.AddAsync(session, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}
