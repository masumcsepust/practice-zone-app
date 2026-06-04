using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
