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
}
