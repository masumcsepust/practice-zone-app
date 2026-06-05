using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using backend.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IUserRepository userRepo,
    IJwtService     jwtService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest(new { message = "Email and password are required." });

        var existing = await userRepo.GetByEmailAsync(req.Email, ct);
        if (existing is not null)
            return Conflict(new { message = "Email already registered." });

        var user = new User
        {
            Id           = Guid.NewGuid(),
            Email        = req.Email.ToLowerInvariant(),
            PasswordHash = PasswordHasher.Hash(req.Password),
            Role         = "User",
            CreatedAt    = DateTime.UtcNow,
            UpdatedAt    = DateTime.UtcNow,
            Profile      = new UserProfile
            {
                Id          = Guid.NewGuid(),
                DisplayName = req.DisplayName,
                CreatedAt   = DateTime.UtcNow,
                UpdatedAt   = DateTime.UtcNow,
            }
        };

        await userRepo.AddAsync(user, ct);

        var token = jwtService.GenerateToken(user);
        return Ok(BuildResponse(token, user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var user = await userRepo.GetByEmailAsync(req.Email, ct);
        if (user is null || !PasswordHasher.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid email or password." });

        var token = jwtService.GenerateToken(user);
        return Ok(BuildResponse(token, user));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(idClaim, out var userId))
            return Unauthorized();

        var user = await userRepo.GetByIdAsync(userId, ct);
        if (user is null) return NotFound();

        var xp            = user.Profile?.TotalXp ?? 0;
        var level         = Math.Max(1, xp / 400 + 1);
        var nextLevelXp   = level * 400;
        var displayName   = user.Profile?.DisplayName ?? "";
        var rawUsername   = user.Profile?.Username;
        var username      = string.IsNullOrWhiteSpace(rawUsername)
            ? "@" + displayName.ToLower().Replace(" ", "_")
            : rawUsername;

        return Ok(new
        {
            user.Id,
            user.Email,
            user.Role,
            DisplayName       = displayName,
            AvatarUrl         = user.Profile?.AvatarUrl          ?? "",
            Username          = username,
            TotalXp           = xp,
            CurrentStreak     = user.Profile?.CurrentStreak      ?? 0,
            TotalLessons      = user.Profile?.TotalLessons       ?? 0,
            CorrectAnswerRate = user.Profile?.CorrectAnswerRate   ?? 0.0,
            Level             = level,
            NextLevelXp       = nextLevelXp,
            MemberSince       = user.CreatedAt.ToString("dd MMMM, yyyy"),
        });
    }

    private static AuthResponse BuildResponse(string token, User user) => new(
        Token:         token,
        UserId:        user.Id,
        DisplayName:   user.Profile?.DisplayName ?? "",
        Email:         user.Email,
        Role:          user.Role,
        TotalXp:       user.Profile?.TotalXp       ?? 0,
        CurrentStreak: user.Profile?.CurrentStreak ?? 0
    );
}
