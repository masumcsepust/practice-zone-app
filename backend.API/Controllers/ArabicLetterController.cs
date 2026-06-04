using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    private readonly ITajweedProgressRepository  _tajweedProgress;
    private readonly ILogger<ArabicLetterController> _logger;

    public ArabicLetterController(
        IArabicLetterRepository     repo,
        IArabicLetterTtsService     tts,
        ILetterExplanationService   explanation,
        ILetterDrawingService       drawing,
        ILetterPronunciationService pronunciation,
        ITajweedProgressRepository  tajweedProgress,
        ILogger<ArabicLetterController> logger)
    {
        _repo            = repo;
        _tts             = tts;
        _explanation     = explanation;
        _drawing         = drawing;
        _pronunciation   = pronunciation;
        _tajweedProgress = tajweedProgress;
        _logger          = logger;
    }

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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();
        return Ok(ToDto(letter));
    }

    [HttpGet("{id:guid}/explanation")]
    public async Task<IActionResult> GetExplanation(Guid id, CancellationToken ct)
    {
        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();

        var dto         = ToDto(letter);
        var explanation = await _explanation.ExplainAsync(dto, ct);
        return Ok(explanation);
    }

    [HttpGet("forms")]
    public async Task<IReadOnlyList<LetterFormsDto>> GetAllForms(CancellationToken ct)
        => await _repo.GetAllFormsAsync(ct);

    [HttpGet("{id:guid}/forms")]
    public async Task<IActionResult> GetFormsById(Guid id, CancellationToken ct)
    {
        var forms = await _repo.GetFormsByIdAsync(id, ct);
        if (forms is null) return NotFound();
        return Ok(forms);
    }

    [HttpPost("{id:guid}/check-drawing")]
    public async Task<IActionResult> CheckDrawing(
        Guid id,
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

    [HttpPost("{id:guid}/check-pronunciation")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> CheckPronunciation(
        Guid           id,
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

        letterDto ??= new ArabicLetterDto(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 1,
            "ا", "Alif", "أَلِف", "আলিফ", "ā / ʾ",
            "Throat", "Makhraj desc", "মাখরাজ বর্ণনা", ["Light"],
            "أَحَد", "One", "এক", "ا", "ا", "ـا", "ـا", false, "",
            Harakat: [], Tanween: []);

        using var ms = new MemoryStream();
        await audio.CopyToAsync(ms, ct);
        var bytes = ms.ToArray();

        var result = await _pronunciation.CheckAsync(letterDto, bytes, audio.ContentType ?? "audio/ogg", ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}/audio")]
    public async Task<IActionResult> GetAudio(Guid id, CancellationToken ct)
    {
        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();

        var audioBytes = await _tts.SynthesizeAsync(letter.NameArabic, ct);
        return File(audioBytes, "audio/mpeg");
    }

    [HttpPost("{id:guid}/tajweed-progress")]
    public async Task<IActionResult> SaveTajweedProgress(
        Guid id,
        [FromBody] SaveTajweedProgressRequest body,
        CancellationToken ct)
    {
        var validModes = new[] { "harakat", "tanween", "sukoon_shaddah", "word_building" };
        if (!validModes.Contains(body.Mode))
            return BadRequest($"Mode must be one of: {string.Join(", ", validModes)}");

        var letter = await _repo.GetByIdAsync(id, ct);
        if (letter is null) return NotFound();

        await _tajweedProgress.SaveAsync(new backend.Domain.Entities.TajweedPracticeRecord
        {
            LetterId      = id,
            Mode          = body.Mode,
            Score         = body.Score,
            AccuracyScore = body.AccuracyScore,
            IsCorrect     = body.IsCorrect,
            PracticedAt   = DateTime.UtcNow,
        }, ct);

        return Ok();
    }

    [HttpGet("tajweed-progress")]
    public async Task<IReadOnlyList<TajweedLetterProgress>> GetTajweedProgress(CancellationToken ct)
        => await _tajweedProgress.GetTableAsync(ct);

    private ArabicLetterDto ToDto(backend.Domain.Entities.ArabicLetter l) => new(
        l.Id,
        l.SequenceOrder,
        l.Character,
        l.Name.En,
        l.NameArabic,
        l.Name.Bn,
        l.Transliteration.En,
        l.MakhrajType,
        l.MakhrajDescription.En,
        l.MakhrajDescription.Bn,
        l.Sifaat.Split(',', StringSplitOptions.RemoveEmptyEntries),
        l.ExampleWordArabic,
        l.ExampleWordMeaning.En,
        l.ExampleWordMeaning.Bn,
        l.IsolatedForm,
        l.InitialForm,
        l.MedialForm,
        l.FinalForm,
        l.IsConnector,
        AudioUrl: $"/api/arabic-letters/{l.Id}/audio",
        Harakat: l.HarakatItems
                  .Where(i => i.Category.Type == "Harakat")
                  .OrderBy(i => i.Category.DisplayOrder)
                  .Select(i => new HarakatItemDto(
                      i.CategoryId, i.Category.Name, i.Category.ArabicName,
                      i.Category.BanglaName, i.Category.Symbol,
                      i.Sound, i.ExampleWord))
                  .ToList(),
        Tanween: l.HarakatItems
                  .Where(i => i.Category.Type == "Tanween")
                  .OrderBy(i => i.Category.DisplayOrder)
                  .Select(i => new HarakatItemDto(
                      i.CategoryId, i.Category.Name, i.Category.ArabicName,
                      i.Category.BanglaName, i.Category.Symbol,
                      i.Sound, i.ExampleWord))
                  .ToList()
    );
}
