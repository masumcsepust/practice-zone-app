using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// LetterFormsDto is in backend.Application.DTOs — same namespace, no extra using needed

namespace backend.API.Controllers;

[ApiController]
[Route("api/arabic-letters")]
public class ArabicLetterController : ControllerBase
{
    private readonly IArabicLetterRepository     _repo;
    private readonly IArabicLetterTtsService     _tts;
    private readonly ILetterExplanationService   _explanation;
    private readonly ILetterDrawingService       _drawing;
    private readonly ILetterPronunciationService _pronunciation;
    private readonly ILogger<ArabicLetterController> _logger;

    public ArabicLetterController(
        IArabicLetterRepository     repo,
        IArabicLetterTtsService     tts,
        ILetterExplanationService   explanation,
        ILetterDrawingService       drawing,
        ILetterPronunciationService pronunciation,
        ILogger<ArabicLetterController> logger)
    {
        _repo          = repo;
        _tts           = tts;
        _explanation   = explanation;
        _drawing       = drawing;
        _pronunciation = pronunciation;
        _logger        = logger;
    }

    /// <summary>
    /// GET /api/arabic-letters?page=1&amp;pageSize=10
    /// Returns one page of letters.  Defaults: page=1, pageSize=10, max pageSize=50.
    /// </summary>
    [HttpGet]
    public async Task<PagedResult<ArabicLetterDto>> GetAll(
        [FromQuery] int page     = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct     = default)
    {
        var paged = await _repo.GetPagedAsync(page, pageSize, ct);
        return new PagedResult<ArabicLetterDto>(
            paged.Items.Select(ToDto).ToList().AsReadOnly(),
            paged.Page,
            paged.PageSize,
            paged.TotalCount);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();
        return Ok(ToDto(letter));
    }

    // ── AI explanation endpoint ───────────────────────────────────────────────

    /// <summary>
    /// GET /api/arabic-letters/{id}/explanation
    /// Uses Semantic Kernel (Azure OpenAI) to produce a detailed bilingual
    /// (English + Bangla) explanation of the letter's makhraj, sifaat, and forms.
    /// </summary>
    [HttpGet("{id:int}/explanation")]
    public async Task<IActionResult> GetExplanation(int id, CancellationToken ct)
    {
        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();

        var dto         = ToDto(letter);
        var explanation = await _explanation.ExplainAsync(dto, ct);
        return Ok(explanation);
    }

    // ── Positional-forms endpoints ────────────────────────────────────────────

    /// <summary>
    /// GET /api/arabic-letters/forms
    /// Returns the four positional forms (isolated, initial, medial, final)
    /// for every letter — lightweight, no makhraj/sifaat data.
    /// </summary>
    [HttpGet("forms")]
    public async Task<IReadOnlyList<LetterFormsDto>> GetAllForms(CancellationToken ct)
        => await _repo.GetAllFormsAsync(ct);

    /// <summary>
    /// GET /api/arabic-letters/{id}/forms
    /// Returns the four positional forms for a single letter.
    /// </summary>
    [HttpGet("{id:int}/forms")]
    public async Task<IActionResult> GetFormsById(int id, CancellationToken ct)
    {
        var forms = await _repo.GetFormsByIdAsync(id, ct);
        if (forms is null) return NotFound();
        return Ok(forms);
    }

    // ─────────────────────────────────────────────────────────────────────────

    // ── Drawing-check endpoint ────────────────────────────────────────────────

    /// <summary>
    /// POST /api/arabic-letters/{id}/check-drawing
    /// Body: { "imageBase64": "&lt;png base64&gt;" }
    /// Uses GPT-4o Vision to evaluate a user's hand-drawn letter.
    /// </summary>
    [HttpPost("{id:int}/check-drawing")]
    public async Task<IActionResult> CheckDrawing(
        int id,
        [FromBody] DrawingCheckRequestDto body,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(body.ImageBase64))
            return BadRequest("imageBase64 is required.");

        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();

        var result = await _drawing.CheckDrawingAsync(ToDto(letter), body.ImageBase64, ct);
        return Ok(result);
    }

    // ── Pronunciation-check endpoint (replaces unreliable WebSocket path) ────

    /// <summary>
    /// POST /api/arabic-letters/{id}/check-pronunciation
    /// Body: multipart/form-data with field "audio" (ogg / webm / wav / mp4).
    /// Uses Azure Speech one-shot recognition + Azure AI evaluation.
    /// </summary>
    [HttpPost("{id:int}/check-pronunciation")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB max
    public async Task<IActionResult> CheckPronunciation(
        int            id,
        IFormFile      audio,
        CancellationToken ct)
    {
        if (audio is null || audio.Length == 0)
            return BadRequest("An audio file field named 'audio' is required.");

        ArabicLetterDto? letterDto = null;
        try 
        {
            var letter = await _repo.GetByIdAsync(id, ct);
            if (letter != null) letterDto = ToDto(letter);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Database failed while fetching letter {Id} for pronunciation check. Using mock fallback.", id);
        }

        // Fallback mock letter if DB is down or letter not found, to allow AI testing
        letterDto ??= new ArabicLetterDto(id, id, "ا", "Alif", "أَلِف", "আলিফ", "ā / ʾ", "Throat", "Makhraj desc", "মাখরাজ বর্ণনা", new[]{"Light"}, "أَحَد", "One", "এক", "ا", "ا", "ـا", "ـا", false, "");

        using var ms = new MemoryStream();
        await audio.CopyToAsync(ms, ct);
        var bytes = ms.ToArray();

        var result = await _pronunciation.CheckAsync(letterDto, bytes, audio.ContentType ?? "audio/ogg", ct);
        return Ok(result);
    }

    // ─────────────────────────────────────────────────────────────────────────

    [HttpGet("{id:int}/audio")]
    public async Task<IActionResult> GetAudio(int id, CancellationToken ct)
    {
        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();

        var audioBytes = await _tts.SynthesizeAsync(letter.NameArabic, ct);
        return File(audioBytes, "audio/mpeg");
    }

    private ArabicLetterDto ToDto(backend.Domain.Entities.ArabicLetter l) => new(
        l.Id,
        l.Order,
        l.Letter,
        l.NameEnglish,
        l.NameArabic,
        l.NameBangla,
        l.Transliteration,
        l.MakhrajType,
        l.MakhrajDescription,
        l.MakhrajDescriptionBn,
        l.Sifaat.Split(',', StringSplitOptions.RemoveEmptyEntries),
        l.ExampleWordArabic,
        l.ExampleWord,
        l.ExampleWordBn,
        l.IsolatedForm,
        l.InitialForm,
        l.MedialForm,
        l.FinalForm,
        l.IsConnector,
        AudioUrl: $"/api/arabic-letters/{l.Id}/audio"
    );
}
