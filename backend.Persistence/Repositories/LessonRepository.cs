using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _db;
    private const int TotalLetters = 28;

    // CategoryIds
    private const int CatHarakat = 2;
    private const int CatTanween = 3;

    public LessonRepository(AppDbContext db) => _db = db;

    // ── Basic CRUD ────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<LessonCategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
        => await _db.LessonCategories
            .OrderBy(c => c.Id)
            .Select(c => new LessonCategoryDto(
                c.Id, c.Name, c.Items.Count,
                c.TitleEn, c.TitleBn, c.DescriptionEn, c.DescriptionBn))
            .ToListAsync(ct);

    public async Task<IReadOnlyList<LessonItemDto>> GetItemsAsync(int categoryId, CancellationToken ct = default)
        => await _db.LessonItems
            .Where(i => i.CategoryId == categoryId)
            .OrderBy(i => i.OrderNo)
            .Select(i => new LessonItemDto(i.Id, i.CategoryId, i.ArabicText, i.SoundText, i.AudioUrl, i.OrderNo, i.DescriptionEn, i.DescriptionBn))
            .ToListAsync(ct);

    public async Task<LessonItemDto?> GetItemByIdAsync(int id, CancellationToken ct = default)
        => await _db.LessonItems
            .Where(i => i.Id == id)
            .Select(i => new LessonItemDto(i.Id, i.CategoryId, i.ArabicText, i.SoundText, i.AudioUrl, i.OrderNo, i.DescriptionEn, i.DescriptionBn))
            .FirstOrDefaultAsync(ct);

    // ── Tanween lesson ────────────────────────────────────────────────────────

    public async Task<TanweenLessonData?> GetTanweenLessonAsync(
        int letterOrder, string baseUrl, CancellationToken ct = default)
    {
        if (letterOrder < 1 || letterOrder > TotalLetters) return null;

        var cat     = await _db.LessonCategories.FindAsync([CatTanween], ct);
        var harakat = await LetterTriplet(CatHarakat, letterOrder, ct);
        var tanween = await LetterTriplet(CatTanween, letterOrder, ct);
        if (harakat.Count < 3 || tanween.Count < 3) return null;

        static string AudioUrl(string baseUrl, string sound)
            => $"{baseUrl}/api/lessons/audio/{Uri.EscapeDataString(sound.ToLowerInvariant())}";

        static string CompareUrl(string baseUrl, string a, string b)
            => $"{baseUrl}/api/lessons/audio/compare/{Uri.EscapeDataString(a.ToLowerInvariant())}/{Uri.EscapeDataString(b.ToLowerInvariant())}";

        var types = new[]
        {
            ("Fatha",    "Fathatan"),
            ("Kasra",    "Kasratan"),
            ("Damma",    "Dammatan"),
        };

        var tanweenTypes = Enumerable.Range(0, 3).Select(i => new TanweenType(
            Id: i + 1,
            BaseLetter:   new TanweenLetter(harakat[i].ArabicText, harakat[i].SoundText, types[i].Item1, AudioUrl(baseUrl, harakat[i].SoundText)),
            TargetLetter: new TanweenLetter(tanween[i].ArabicText, tanween[i].SoundText, types[i].Item2, AudioUrl(baseUrl, tanween[i].SoundText))
        )).ToList();

        var listens = Enumerable.Range(0, 3).Select(i => new ListenCompare(
            Label:    $"{harakat[i].SoundText} vs {tanween[i].SoundText}",
            AudioUrl: CompareUrl(baseUrl, harakat[i].SoundText, tanween[i].SoundText)
        )).ToList();

        var first = tanweenTypes[0];

        return new TanweenLessonData(
            Header: new LessonHeader(
                Title:              cat?.TitleEn ?? "Tanween Learning",
                CurrentLesson:      letterOrder,
                TotalLessons:       TotalLetters,
                ProgressPercentage: (letterOrder - 1) * 100 / TotalLetters
            ),
            Concept: new LessonConcept(
                TitleEn:       cat?.TitleEn       ?? "",
                TitleBn:       cat?.TitleBn       ?? "",
                DescriptionEn: cat?.DescriptionEn ?? "",
                DescriptionBn: cat?.DescriptionBn ?? "",
                Example: new ConceptExample(
                    BaseText:   first.BaseLetter.Transliteration,
                    ResultText: first.TargetLetter.Transliteration,
                    ResultType: first.TargetLetter.VowelType,
                    AudioUrl:   first.TargetLetter.AudioUrl
                )
            ),
            TanweenTypes:     tanweenTypes,
            ListenAndCompare: listens
        );
    }

    // ── Harakat lesson ────────────────────────────────────────────────────────

    public async Task<HarakatLessonData?> GetHarakatLessonAsync(
        int letterOrder, string baseUrl, CancellationToken ct = default)
    {
        if (letterOrder < 1 || letterOrder > TotalLetters) return null;

        var cat     = await _db.LessonCategories.FindAsync([CatHarakat], ct);
        var harakat = await LetterTriplet(CatHarakat, letterOrder, ct);
        if (harakat.Count < 3) return null;

        static string AudioUrl(string baseUrl, string sound)
            => $"{baseUrl}/api/lessons/audio/{Uri.EscapeDataString(sound.ToLowerInvariant())}";

        var vowelTypes = new[] { "Fatha", "Kasra", "Damma" };

        var harakatTypes = Enumerable.Range(0, 3).Select(i => new HarakatType(
            Id: i + 1,
            Letter: new HarakatLetter(
                harakat[i].ArabicText,
                harakat[i].SoundText,
                vowelTypes[i],
                AudioUrl(baseUrl, harakat[i].SoundText))
        )).ToList();

        var listens = Enumerable.Range(0, 3).Select(i => new ListenCompare(
            Label:    harakat[i].SoundText,
            AudioUrl: AudioUrl(baseUrl, harakat[i].SoundText)
        )).ToList();

        return new HarakatLessonData(
            Header: new LessonHeader(
                Title:              cat?.TitleEn ?? "Harakat Learning",
                CurrentLesson:      letterOrder,
                TotalLessons:       TotalLetters,
                ProgressPercentage: (letterOrder - 1) * 100 / TotalLetters
            ),
            Concept: new LessonConcept(
                TitleEn:       cat?.TitleEn       ?? "",
                TitleBn:       cat?.TitleBn       ?? "",
                DescriptionEn: cat?.DescriptionEn ?? "",
                DescriptionBn: cat?.DescriptionBn ?? "",
                Example: new ConceptExample(
                    BaseText:   harakatTypes[0].Letter.Transliteration,
                    ResultText: harakatTypes[1].Letter.Transliteration,
                    ResultType: harakatTypes[1].Letter.VowelType,
                    AudioUrl:   harakatTypes[0].Letter.AudioUrl
                )
            ),
            HarakatTypes:    harakatTypes,
            ListenAndCompare: listens
        );
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Fetches the fatha/kasra/damma (or tanween fath/kasr/damm) triplet for a
    /// given letter position. In LessonItems each letter occupies 3 consecutive
    /// OrderNo values starting at (letterOrder-1)*3 + 1.
    /// </summary>
    private async Task<List<LessonItem>> LetterTriplet(
        int categoryId, int letterOrder, CancellationToken ct)
    {
        int startOrder = (letterOrder - 1) * 3 + 1;
        return await _db.LessonItems
            .Where(i => i.CategoryId == categoryId
                     && i.OrderNo >= startOrder
                     && i.OrderNo <= startOrder + 2)
            .OrderBy(i => i.OrderNo)
            .ToListAsync(ct);
    }
}
