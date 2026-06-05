using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonController : ControllerBase
{
    private readonly ILessonRepository _repo;

    public LessonController(ILessonRepository repo) => _repo = repo;

    [HttpGet("tanween/{letterOrder:int}")]
    public async Task<IActionResult> GetTanweenLesson(int letterOrder, CancellationToken ct)
    {
        var data = await _repo.GetTanweenLessonAsync(letterOrder, BaseUrl(), ct);
        if (data is null) return NotFound(new { message = $"Letter order {letterOrder} is out of range (1–28)." });
        return Ok(new LessonResponse<TanweenLessonData>("success", data));
    }

    private string BaseUrl()
        => $"{Request.Scheme}://{Request.Host}";
}
