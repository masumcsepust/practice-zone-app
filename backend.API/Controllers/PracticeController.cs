using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[ApiController]
[Route("api/practice/lessons")]
public class PracticeController(ILessonPracticeRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var lessons = await repo.GetAllAsync(ct);
        return Ok(new { status = "success", data = lessons });
    }

    [HttpGet("{lessonId:guid}")]
    public async Task<IActionResult> GetById(Guid lessonId, CancellationToken ct)
    {
        var data = await repo.GetByIdAsync(lessonId, ct);
        if (data is null) return NotFound();
        return Ok(new PracticeSessionResponse("success", data));
    }

    // GET /api/practice/lessons/{lessonNum}/step/{stepNum}
    [HttpGet("{lessonNum:int}/step/{stepNum:int}")]
    public async Task<IActionResult> GetStep(int lessonNum, int stepNum, CancellationToken ct)
    {
        var step = await repo.GetStepAsync(lessonNum, stepNum, ct);
        if (step is null) return NotFound();
        return Ok(step);
    }
}
