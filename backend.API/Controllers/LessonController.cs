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

    // ── Category & item endpoints ─────────────────────────────────────────────

    /// <summary>GET /api/lessons/categories — all categories with item counts.</summary>
    [HttpGet("categories")]
    public async Task<IReadOnlyList<LessonCategoryDto>> GetCategories(CancellationToken ct)
        => await _repo.GetCategoriesAsync(ct);

    /// <summary>
    /// GET /api/lessons/categories/{categoryId}/items
    /// categoryId: 1=Arabic Letters 2=Harakat 3=Tanween 4=Sukoon 5=Shaddah 6=Word Building
    /// </summary>
    [HttpGet("categories/{categoryId:int}/items")]
    public async Task<IActionResult> GetItems(int categoryId, CancellationToken ct)
    {
        var items = await _repo.GetItemsAsync(categoryId, ct);
        return items.Count == 0 ? NotFound() : Ok(items);
    }

    /// <summary>GET /api/lessons/items/{id} — single lesson item.</summary>
    [HttpGet("items/{id:int}")]
    public async Task<IActionResult> GetItem(int id, CancellationToken ct)
    {
        var item = await _repo.GetItemByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // ── Structured lesson endpoints ───────────────────────────────────────────

    /// <summary>
    /// GET /api/lessons/tanween/{letterOrder}
    /// Returns a full structured tanween lesson for the given Arabic letter (1–28).
    /// letterOrder 1 = Alif, 2 = Ba, …, 28 = Ya.
    /// </summary>
    [HttpGet("tanween/{letterOrder:int}")]
    public async Task<IActionResult> GetTanweenLesson(int letterOrder, CancellationToken ct)
    {
        var data = await _repo.GetTanweenLessonAsync(letterOrder, BaseUrl(), ct);
        if (data is null) return NotFound(new { message = $"Letter order {letterOrder} is out of range (1–28)." });
        return Ok(new LessonResponse<TanweenLessonData>("success", data));
    }

    /// <summary>
    /// GET /api/lessons/harakat/{letterOrder}
    /// Returns a full structured harakat lesson for the given Arabic letter (1–28).
    /// </summary>
    [HttpGet("harakat/{letterOrder:int}")]
    public async Task<IActionResult> GetHarakatLesson(int letterOrder, CancellationToken ct)
    {
        var data = await _repo.GetHarakatLessonAsync(letterOrder, BaseUrl(), ct);
        if (data is null) return NotFound(new { message = $"Letter order {letterOrder} is out of range (1–28)." });
        return Ok(new LessonResponse<HarakatLessonData>("success", data));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private string BaseUrl()
        => $"{Request.Scheme}://{Request.Host}";
}
