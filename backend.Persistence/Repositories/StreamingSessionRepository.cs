using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class StreamingSessionRepository
{
    private readonly AppDbContext _ctx;

    public StreamingSessionRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<StreamingSession> CreateAsync(int userId, string connectionId, CancellationToken ct = default)
    {
        var session = new StreamingSession
        {
            UserId       = userId,
            ConnectionId = connectionId,
            StartedAt    = DateTime.UtcNow,
            Status       = "Active"
        };
        await _ctx.StreamingSessions.AddAsync(session, ct);
        await _ctx.SaveChangesAsync(ct);
        return session;
    }

    public async Task CompleteAsync(string connectionId, string status = "Completed", CancellationToken ct = default)
    {
        var session = await _ctx.StreamingSessions
            .FirstOrDefaultAsync(s => s.ConnectionId == connectionId, ct);

        if (session is null) return;

        session.EndedAt = DateTime.UtcNow;
        session.Status  = status;
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task SaveFinalResultAsync(int streamingSessionId, string text, CancellationToken ct = default)
    {
        await _ctx.SpeechRecognitionResults.AddAsync(new SpeechRecognitionResult
        {
            StreamingSessionId = streamingSessionId,
            RecognizedText     = text,
            ResultType         = "Final",
            RecognizedAt       = DateTime.UtcNow
        }, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}
