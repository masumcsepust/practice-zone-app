using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[ApiController]
[Route("api/quran")]
public class QuranController : ControllerBase
{
    private readonly ISurahRepository _surahs;
    private readonly IAyahRepository  _ayahs;

    public QuranController(ISurahRepository surahs, IAyahRepository ayahs)
    {
        _surahs = surahs;
        _ayahs  = ayahs;
    }

    [HttpGet("surahs")]
    public async Task<IReadOnlyList<SurahDto>> GetSurahs(CancellationToken ct)
    {
        var surahs = await _surahs.GetAllAsync(ct);
        return surahs.Select(s => new SurahDto(
            s.Id, s.SurahNumber, s.NameArabic, s.NameEnglish, s.NameBangla, s.TotalAyahs
        )).ToList();
    }

    [HttpGet("surahs/{surahId:int}/ayahs")]
    public async Task<IActionResult> GetAyahs(int surahId, CancellationToken ct)
    {
        var surah = await _surahs.GetByIdAsync(surahId, ct);
        if (surah is null) return NotFound();

        var ayahs = await _ayahs.GetBySurahIdAsync(surahId, ct);
        var dtos = ayahs.Select(a => new AyahDto(
            a.Id, a.SurahId, a.AyahNumber,
            a.ArabicText, a.EnglishTranslation, a.BanglaTranslation, a.Transliteration
        )).ToList();

        return Ok(dtos);
    }
}
