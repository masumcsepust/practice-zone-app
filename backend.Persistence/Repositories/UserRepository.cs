using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => db.Users.Include(u => u.Profile)
                   .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Users.Include(u => u.Profile)
                   .FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync(ct);
    }

    public async Task<LeaderboardResponseDto> GetLeaderboardAsync(string period, Guid? currentUserId, CancellationToken ct = default)
    {
        // Build ranked list of (UserId, DisplayName, AvatarUrl, Xp)
        List<(Guid UserId, string DisplayName, string AvatarUrl, int Xp)> allRanked;

        if (period is "week" or "month")
        {
            var since = period == "week"
                ? DateTime.UtcNow.AddDays(-7)
                : DateTime.UtcNow.AddDays(-30);

            var grouped = await db.UserXpLogs
                .Where(x => x.EarnedAt >= since)
                .GroupBy(x => x.UserId)
                .Select(g => new { UserId = g.Key, Xp = g.Sum(x => x.XpEarned) })
                .ToListAsync(ct);

            var profiles = await db.UserProfiles
                .Select(p => new { p.UserId, p.DisplayName, p.AvatarUrl })
                .ToListAsync(ct);

            allRanked = grouped
                .Join(profiles, g => g.UserId, p => p.UserId,
                      (g, p) => (g.UserId, p.DisplayName, p.AvatarUrl, g.Xp))
                .OrderByDescending(x => x.Xp)
                .ToList();
        }
        else
        {
            allRanked = await db.UserProfiles
                .OrderByDescending(p => p.TotalXp)
                .Select(p => new { p.UserId, p.DisplayName, p.AvatarUrl, Xp = p.TotalXp })
                .ToListAsync(ct)
                .ContinueWith(t => t.Result
                    .Select(x => (x.UserId, x.DisplayName, x.AvatarUrl, x.Xp))
                    .ToList(), ct);
        }

        var entries = allRanked
            .Take(50)
            .Select((x, i) => new LeaderboardEntryDto(i + 1, x.DisplayName, x.AvatarUrl, x.Xp))
            .ToList();

        LeaderboardMyPositionDto? myPosition = null;
        if (currentUserId.HasValue)
        {
            var myIndex = allRanked.FindIndex(x => x.UserId == currentUserId.Value);
            if (myIndex >= 0)
            {
                var me       = allRanked[myIndex];
                var myRank   = myIndex + 1;
                var aboveXp  = myIndex > 0 ? allRanked[myIndex - 1].Xp : me.Xp;
                var xpToNext = myIndex > 0 ? Math.Max(0, aboveXp - me.Xp + 1) : 0;
                myPosition = new LeaderboardMyPositionDto(myRank, me.DisplayName, me.AvatarUrl, me.Xp, xpToNext);
            }
            else
            {
                // User has no XP yet — put them at the end
                var profile = await db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == currentUserId.Value, ct);
                if (profile is not null)
                {
                    var myXp    = period is "week" or "month" ? 0 : profile.TotalXp;
                    var myRank  = allRanked.Count + 1;
                    var lastXp  = allRanked.Count > 0 ? allRanked[^1].Xp : 0;
                    var xpToNext = Math.Max(0, lastXp - myXp + 1);
                    myPosition = new LeaderboardMyPositionDto(myRank, profile.DisplayName, profile.AvatarUrl, myXp, xpToNext);
                }
            }
        }

        return new LeaderboardResponseDto(entries, myPosition);
    }
}
