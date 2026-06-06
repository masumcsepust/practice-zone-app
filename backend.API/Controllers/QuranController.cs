using System.Text.Json;
using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[ApiController]
[Route("api/quran")]
public class QuranController : ControllerBase
{
    private readonly ISurahRepository              _surahs;
    private readonly IAyahRepository               _ayahs;
    private readonly HttpClient                    _http;
    private readonly IPronunciationAssessmentService _pronunciation;
    private readonly ITajweedEngine                _tajweedEngine;

    private static readonly string AlQuranBase   = "https://api.alquran.cloud/v1/surah";
    private static readonly string ArabicEdition = "quran-simple";
    private static readonly string EnglishEdition= "en.sahih";

    // Al-Fātiḥah + 9 short surahs from Juz 30 — good for recitation practice
    private static readonly int[] RecitationSurahNumbers =
        { 1, 103, 106, 108, 109, 110, 111, 112, 113, 114 };

    public QuranController(
        ISurahRepository               surahs,
        IAyahRepository                ayahs,
        IHttpClientFactory             httpFactory,
        IPronunciationAssessmentService pronunciation,
        ITajweedEngine                 tajweedEngine)
    {
        _surahs        = surahs;
        _ayahs         = ayahs;
        _http          = httpFactory.CreateClient("alquran");
        _pronunciation = pronunciation;
        _tajweedEngine = tajweedEngine;
    }

    [HttpGet("surahs")]
    public async Task<IReadOnlyList<SurahDto>> GetSurahs(CancellationToken ct)
    {
        var surahs = await _surahs.GetAllAsync(ct);
        return surahs.Select(s => new SurahDto(
            s.Id, s.SurahNumber, s.NameArabic, s.NameEnglish, s.NameBangla, s.TotalAyahs
        )).ToList();
    }

    /// <summary>
    /// Returns 10 curated surahs for recitation practice. Seeds from Al-Quran Cloud on first call.
    /// NameEnglish = transliteration (e.g. "Al-Faatiha"), NameBangla = English meaning ("The Opening").
    /// </summary>
    [HttpGet("surahs/recitation")]
    public async Task<IReadOnlyList<SurahDto>> GetRecitationSurahs(CancellationToken ct)
    {
        var result = new List<SurahDto>(RecitationSurahNumbers.Length);
        foreach (var num in RecitationSurahNumbers)
        {
            var surah = await _surahs.GetBySurahNumberAsync(num, ct)
                        ?? await FetchAndCacheSurahMetaAsync(num, ct);
            if (surah is not null)
                result.Add(new SurahDto(surah.Id, surah.SurahNumber,
                    surah.NameArabic, surah.NameEnglish, surah.NameBangla, surah.TotalAyahs));
        }
        return result;
    }

    [HttpGet("surahs/{surahId:int}/ayahs")]
    public async Task<IActionResult> GetAyahs(int surahId, CancellationToken ct)
    {
        var surah = await _surahs.GetByIdAsync(surahId, ct);
        if (surah is null) return NotFound();

        var ayahs = await _ayahs.GetBySurahIdAsync(surahId, ct);

        if (ayahs.Count == 0)
            ayahs = await FetchAndCacheFromAlQuranAsync(surah, ct);

        var dtos = ayahs.Select(ToDto).ToList();

        return Ok(dtos);
    }

    [HttpGet("pages/{pageNumber:int}")]
    public async Task<IActionResult> GetPage(int pageNumber, CancellationToken ct)
    {
        if (pageNumber < 1 || pageNumber > 604) return BadRequest("Page must be between 1 and 604.");

        var ayahs = await _ayahs.GetByPageAsync(pageNumber, ct);

        if (ayahs.Count == 0)
            ayahs = await FetchAndCachePageFromAlQuranAsync(pageNumber, ct);

        return Ok(ayahs.Select(ToDto).ToList());
    }

    // ── Ayah recitation assessment ────────────────────────────────────────────

    [HttpPost("surahs/{surahId:int}/ayahs/{ayahNumber:int}/assess")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> AssessAyahRecitation(
        int surahId, int ayahNumber, IFormFile audio, CancellationToken ct)
    {
        var ayah = await _ayahs.GetBySurahAndAyahAsync(surahId, ayahNumber, ct);
        if (ayah is null) return NotFound("Ayah not found");

        using var ms = new MemoryStream();
        await audio.CopyToAsync(ms, ct);

        var pronunciation = await _pronunciation.AssessOnceAsync(
            ayah.ArabicText, ms.ToArray(), audio.ContentType ?? "audio/ogg", ct);

        if (pronunciation is null)
            return Ok(new AyahAssessmentDto(0, 0, 0f, "",
                [new AyahNoteDto("POSITIVE", "", "No speech detected",
                    "Make sure your microphone is working and speak clearly.", false, null)]));

        var analysis = _tajweedEngine.Analyze(pronunciation, ayah.ArabicText);

        return Ok(new AyahAssessmentDto(
            OverallTajweed: (int)Math.Round(analysis.TajweedScore),
            LettersPct:     (int)Math.Round(analysis.AccuracyScore),
            PacePct:        (float)(analysis.FluencyScore / 100.0),
            RecognizedText: analysis.RecognizedText,
            Notes:          BuildNotes(analysis)));
    }

    private static IReadOnlyList<AyahNoteDto> BuildNotes(TajweedAnalysisResponseDto analysis)
    {
        var notes = new List<AyahNoteDto>();

        foreach (var issue in analysis.TajweedIssues)
        {
            var kind = issue.RuleType switch
            {
                "Qalqalah" => "QALQALAH",
                "Madd"     => "MADD",
                "Ghunnah"  => "GHUNNAH",
                "Idgham"   => "IDGHAAM",
                "Ikhfa"    => "IKHFAA",
                _          => "QALQALAH"
            };
            notes.Add(new AyahNoteDto(kind, issue.AffectedLetter,
                $"{issue.RuleType} — \"{issue.Word}\"",
                issue.Feedback, false, null));
        }

        foreach (var word in analysis.Words.Where(w => !w.IsCorrect && w.AccuracyScore < 70))
        {
            if (notes.Any(n => n.Arabic == word.Word)) continue;
            notes.Add(new AyahNoteDto("QALQALAH", word.Word,
                $"Check \"{word.Word}\"",
                $"Accuracy {word.AccuracyScore:F0}% — error: {word.ErrorType}.", false, null));
        }

        if (analysis.FluencyScore < 60)
            notes.Add(new AyahNoteDto("MADD", "", "Fluency",
                "Try to maintain a steady pace without pausing mid-word.", false, null));

        if (analysis.TajweedScore >= 75 || notes.Count == 0)
            notes.Add(new AyahNoteDto("POSITIVE", "",
                analysis.TajweedScore >= 90 ? "Mā shā' Allāh — excellent!" : "Good effort!",
                analysis.TajweedScore >= 90
                    ? "Your pronunciation and tajweed were very clear."
                    : "You're making good progress. Keep practicing.",
                true, null));

        return notes;
    }

    private async Task<Surah?> FetchAndCacheSurahMetaAsync(int surahNumber, CancellationToken ct)
    {
        try
        {
            var resp = await _http.GetStringAsync($"{AlQuranBase}/{surahNumber}", ct);
            using var doc  = JsonDocument.Parse(resp);
            var data = doc.RootElement.GetProperty("data");

            var surah = new Surah
            {
                Id          = surahNumber,
                SurahNumber = surahNumber,
                NameArabic  = data.GetProperty("name").GetString()                     ?? "",
                NameEnglish = data.GetProperty("englishName").GetString()              ?? "",
                NameBangla  = data.GetProperty("englishNameTranslation").GetString()   ?? "",
                TotalAyahs  = data.GetProperty("numberOfAyahs").GetInt32(),
            };

            await _surahs.UpsertAsync(surah, ct);
            return surah;
        }
        catch
        {
            return null;
        }
    }

    private async Task<IReadOnlyList<Ayah>> FetchAndCacheFromAlQuranAsync(
        Surah surah, CancellationToken ct)
    {
        // Fetch Arabic text (simple enhanced — standard Unicode, renders correctly on all fonts)
        var arabicResp = await _http.GetStringAsync($"{AlQuranBase}/{surah.SurahNumber}/{ArabicEdition}", ct);
        using var arabicDoc = JsonDocument.Parse(arabicResp);
        var arabicAyahs = arabicDoc.RootElement
            .GetProperty("data").GetProperty("ayahs").EnumerateArray().ToList();

        // Fetch English translation (Sahih International)
        var enResp = await _http.GetStringAsync($"{AlQuranBase}/{surah.SurahNumber}/{EnglishEdition}", ct);
        using var enDoc = JsonDocument.Parse(enResp);
        var enAyahs = enDoc.RootElement
            .GetProperty("data").GetProperty("ayahs").EnumerateArray().ToList();

        // Use a unique offset: surahNumber * 10000 so IDs never collide across surahs
        int baseId = surah.SurahNumber * 10_000;

        var entities = arabicAyahs.Select((a, i) =>
        {
            var arabicText = (a.GetProperty("text").GetString() ?? "").Trim().TrimStart('﻿');
            var enText     = i < enAyahs.Count
                ? (enAyahs[i].GetProperty("text").GetString() ?? "").Trim()
                : "";
            int ayahNum    = a.GetProperty("numberInSurah").GetInt32();

            return new Ayah
            {
                Id                   = baseId + ayahNum,
                SurahId              = surah.Id,
                AyahNumber           = ayahNum,
                ArabicText           = arabicText,
                NormalizedArabicText = Normalize(arabicText),
                EnglishTranslation   = enText,
                BanglaTranslation    = "",
                Transliteration      = "",
                Page                 = a.TryGetProperty("page",         out var pg)   ? pg.GetInt32()    : null,
                Juz                  = a.TryGetProperty("juz",          out var jz)   ? jz.GetInt32()    : null,
                Manzil               = a.TryGetProperty("manzil",       out var mz)   ? mz.GetInt32()    : null,
                Ruku                 = a.TryGetProperty("ruku",         out var rk)   ? rk.GetInt32()    : null,
                HizbQuarter          = a.TryGetProperty("hizbQuarter",  out var hq)   ? hq.GetInt32()    : null,
                Sajda                = a.TryGetProperty("sajda",        out var sd)   ? sd.GetBoolean()  : null,
            };
        }).ToList();

        await _ayahs.AddRangeAsync(entities, ct);
        return entities;
    }

    private async Task<IReadOnlyList<Ayah>> FetchAndCachePageFromAlQuranAsync(
        int pageNumber, CancellationToken ct)
    {
        var arabicResp = await _http.GetStringAsync(
            $"https://api.alquran.cloud/v1/page/{pageNumber}/{ArabicEdition}", ct);
        using var arabicDoc = JsonDocument.Parse(arabicResp);
        var arabicAyahs = arabicDoc.RootElement
            .GetProperty("data").GetProperty("ayahs").EnumerateArray().ToList();

        var enResp = await _http.GetStringAsync(
            $"https://api.alquran.cloud/v1/page/{pageNumber}/{EnglishEdition}", ct);
        using var enDoc = JsonDocument.Parse(enResp);
        var enAyahs = enDoc.RootElement
            .GetProperty("data").GetProperty("ayahs").EnumerateArray()
            .ToDictionary(a => a.GetProperty("number").GetInt32());

        var surahCache = new Dictionary<int, Surah>();

        var entities = new List<Ayah>();
        foreach (var a in arabicAyahs)
        {
            var surahNumber = a.GetProperty("surah").GetProperty("number").GetInt32();
            if (!surahCache.TryGetValue(surahNumber, out var surah))
            {
                surah = await _surahs.GetBySurahNumberAsync(surahNumber, ct);
                if (surah is null) continue;
                surahCache[surahNumber] = surah;
            }

            int globalNumber = a.GetProperty("number").GetInt32();
            int ayahNum      = a.GetProperty("numberInSurah").GetInt32();
            var arabicText   = (a.GetProperty("text").GetString() ?? "").Trim().TrimStart('﻿');
            var enText       = enAyahs.TryGetValue(globalNumber, out var en)
                ? (en.GetProperty("text").GetString() ?? "").Trim()
                : "";

            entities.Add(new Ayah
            {
                Id                   = surah.SurahNumber * 10_000 + ayahNum,
                SurahId              = surah.Id,
                AyahNumber           = ayahNum,
                ArabicText           = arabicText,
                NormalizedArabicText = Normalize(arabicText),
                EnglishTranslation   = enText,
                BanglaTranslation    = "",
                Transliteration      = "",
                Page                 = a.TryGetProperty("page",        out var pg) ? pg.GetInt32()   : pageNumber,
                Juz                  = a.TryGetProperty("juz",         out var jz) ? jz.GetInt32()   : null,
                Manzil               = a.TryGetProperty("manzil",      out var mz) ? mz.GetInt32()   : null,
                Ruku                 = a.TryGetProperty("ruku",        out var rk) ? rk.GetInt32()   : null,
                HizbQuarter          = a.TryGetProperty("hizbQuarter", out var hq) ? hq.GetInt32()   : null,
                Sajda                = a.TryGetProperty("sajda",       out var sd) ? sd.GetBoolean() : null,
            });
        }

        await _ayahs.AddRangeAsync(entities, ct);
        return entities;
    }

    private static AyahDto ToDto(Ayah a) => new(
        a.Id, a.SurahId, a.AyahNumber,
        a.ArabicText, a.EnglishTranslation, a.BanglaTranslation, a.Transliteration,
        a.Page, a.Juz, a.Manzil, a.Ruku, a.HizbQuarter, a.Sajda,
        $"https://everyayah.com/data/Alafasy_128kbps/{a.SurahId:D3}{a.AyahNumber:D3}.mp3");

    private static string Normalize(string arabic)
        => new string(arabic.Where(c => c >= '؀' && c <= 'ۿ').ToArray());
}
