namespace backend.Application.DTOs;

public record RegisterRequest(string Email, string Password, string DisplayName);

public record LoginRequest(string Email, string Password);

public record AuthResponse(
    string Token,
    Guid   UserId,
    string DisplayName,
    string Email,
    string Role,
    int    TotalXp,
    int    CurrentStreak
);
