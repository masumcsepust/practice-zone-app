using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;

namespace backend.API.Controllers;

[ApiController]
[Route("api/diacritic-signs")]
public class DiacriticSignController : ControllerBase
{
    private readonly AppDbContext _db;
    public DiacriticSignController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var signs = await _db.DiacriticSigns
            .OrderBy(s => s.SignGroup)
            .Select(s => new { s.Id, s.Symbol, NameEn = s.Name.En, NameBn = s.Name.Bn, s.SignGroup })
            .ToListAsync(ct);
        return Ok(signs);
    }
}

[ApiController]
[Route("api/syllable-sounds")]
public class SyllableSoundController : ControllerBase
{
    private readonly ISyllableSoundRepository _repo;

    public SyllableSoundController(ISyllableSoundRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<PagedResult<SyllableSoundDto>> GetAll(
        [FromQuery] int page     = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct     = default)
    {
        var paged = await _repo.GetPagedAsync(page, pageSize, ct);
        return new PagedResult<SyllableSoundDto>(
            paged.Items.Select(ToDto).ToList().AsReadOnly(),
            paged.Page, paged.PageSize, paged.TotalCount);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return entity is null ? NotFound() : Ok(ToDto(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSyllableSoundDto body,
        CancellationToken ct)
    {
        var entity = new SyllableSound
        {
            LetterId          = body.LetterId,
            SignId             = body.SignId,
            CombinedCharacter  = body.CombinedCharacter,
            Transliteration    = new LocalizedText
            {
                En = body.TransliterationEn,
                Bn = body.TransliterationBn
            },
            AudioUrl = body.AudioUrl
        };

        var created = await _repo.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSyllableSoundDto body,
        CancellationToken ct)
    {
        var updated = await _repo.UpdateAsync(id, body, ct);
        return updated is null ? NotFound() : Ok(ToDto(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _repo.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static SyllableSoundDto ToDto(SyllableSound s) => new(
        s.Id,
        s.LetterId,
        s.Letter.Character,
        s.Letter.Name.En,
        s.SignId,
        s.Sign.Symbol,
        s.Sign.Name.En,
        s.CombinedCharacter,
        s.Transliteration.En,
        s.Transliteration.Bn,
        s.AudioUrl
    );
}
