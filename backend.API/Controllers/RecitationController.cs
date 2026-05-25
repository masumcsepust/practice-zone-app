using backend.API.Models;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[ApiController]
[Route("api/recitation")]
public class RecitationController : ControllerBase
{
    private readonly IRecitationService _recitationService;

    public RecitationController(IRecitationService recitationService)
        => _recitationService = recitationService;

    [HttpPost("recognize")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public async Task<IActionResult> Recognize(
        [FromForm] RecognizeFormRequest form,
        CancellationToken ct = default)
    {
        if (form.AudioFile is null || form.AudioFile.Length == 0)
            return BadRequest(new { error = "Audio file is required." });

        var request = new RecognizeRecitationRequest
        {
            AyahId = form.AyahId,
            UserId = form.UserId,
            AudioStream = form.AudioFile.OpenReadStream(),
            FileName = form.AudioFile.FileName
        };

        var result = await _recitationService.ProcessAsync(request, ct);
        return Ok(result);
    }
}
