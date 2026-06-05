using backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController(IUserRepository userRepo) : ControllerBase
{
    /// <summary>
    /// GET /api/leaderboard?period=all|week|month
    /// Returns top-50 ranked users and (if authenticated) the caller's position.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(
        [FromQuery] string period = "all",
        CancellationToken ct = default)
    {
        if (period is not ("all" or "week" or "month"))
            return BadRequest(new { message = "period must be all, week, or month" });

        Guid? currentUserId = null;
        var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(idClaim, out var parsedId))
            currentUserId = parsedId;

        var result = await userRepo.GetLeaderboardAsync(period, currentUserId, ct);
        return Ok(result);
    }
}
