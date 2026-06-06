using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence.Repositories;

public class LessonPracticeRepository(AppDbContext db) : ILessonPracticeRepository
{
    public async Task<PracticeSessionData?> GetByIdAsync(Guid lessonId, CancellationToken ct = default)
    {
        var lesson = await QueryWithIncludes()
            .FirstOrDefaultAsync(l => l.Id == lessonId, ct);

        return lesson is null ? null : ToDto(lesson);
    }

    public async Task<IReadOnlyList<PracticeSessionData>> GetAllAsync(CancellationToken ct = default)
    {
        var lessons = await QueryWithIncludes()
            .OrderBy(l => l.SequenceOrder)
            .ToListAsync(ct);

        return lessons.Select(ToDto).ToList().AsReadOnly();
    }

    private IQueryable<Lesson> QueryWithIncludes()
        => db.Lessons
             // TargetSyllable is required — always load Letter and Sign
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.TargetSyllable).ThenInclude(s => s.Letter)
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.TargetSyllable).ThenInclude(s => s.Sign)
             // CompareWithSyllable is optional — EF generates LEFT JOINs;
             // Letter and Sign are only populated when CompareWithSyllableId != null
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.CompareWithSyllable)
                 .ThenInclude(s => s!.Letter)
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.CompareWithSyllable)
                 .ThenInclude(s => s!.Sign);

    private static PracticeSessionData ToDto(Lesson l) => new(
        LessonId:      l.Id,
        Title:         l.Title.Bn.Length > 0 ? l.Title.Bn : l.Title.En,
        SequenceOrder: l.SequenceOrder,
        PracticeItems: l.PracticeItems
            .OrderBy(p => p.Id)
            .Select(p => new PracticeItemSessionDto(
                Id:                 p.Id,
                Instruction:        p.Instruction.Bn.Length > 0 ? p.Instruction.Bn : p.Instruction.En,
                TargetSyllable:     ToSyllableDto(p.TargetSyllable),
                CompareWithSyllable: p.CompareWithSyllable is null ? null : ToSyllableDto(p.CompareWithSyllable),
                SuccessTip:         p.SuccessTip.Bn.Length > 0 ? p.SuccessTip.Bn : p.SuccessTip.En
            ))
            .ToList()
            .AsReadOnly()
    );

    public async Task<PracticeStepDto?> GetStepAsync(int lessonNum, int stepNum, CancellationToken ct = default)
    {
        var totalLessons = await db.Lessons.CountAsync(ct);

        var lesson = await QueryWithIncludes()
            .Where(l => l.SequenceOrder == lessonNum)
            .FirstOrDefaultAsync(ct);

        if (lesson is null) return null;

        var items      = lesson.PracticeItems.OrderBy(p => p.Id).ToList();
        var totalSteps = items.Count;

        if (stepNum < 1 || stepNum > totalSteps) return null;

        var item = items[stepNum - 1];

        // Global progress: (completed items across all previous lessons + current step - 1) / total items
        var totalItems      = await db.PracticeItems.CountAsync(ct);
        var completedBefore = await db.Lessons
            .Where(l => l.SequenceOrder < lessonNum)
            .Select(l => l.PracticeItems.Count)
            .SumAsync(ct);
        var progress = totalItems == 0 ? 0
            : (int)Math.Round((completedBefore + stepNum - 1) * 100.0 / totalItems);

        var title = lesson.Title.Bn.Length > 0 ? lesson.Title.Bn : lesson.Title.En;
        var tip   = item.SuccessTip.Bn.Length > 0 ? item.SuccessTip.Bn : item.SuccessTip.En;

        return new PracticeStepDto(
            Id:           stepNum,
            Title:        title,
            LessonNum:    lessonNum,
            Progress:     progress,
            Tips:         tip,
            Left:         ToStepSyllable(item.TargetSyllable,      item.Instruction.Bn.Length > 0 ? item.Instruction.Bn : item.Instruction.En),
            Right:        item.CompareWithSyllable is null ? null : ToStepSyllable(item.CompareWithSyllable, "তুলনা করতে রেকর্ড করুন"),
            RememberText: tip,
            TotalLessons: totalLessons,
            TotalSteps:   totalSteps
        );
    }

    private static StepSyllableDto ToStepSyllable(SyllableSound s, string prompt) => new(
        Arabic:              s.CombinedCharacter,
        Translit:            s.Transliteration.En,
        Bengali:             s.Transliteration.Bn,
        TransliterationText: s.TransliterationText,
        Prompt:              prompt,
        LetterBengali:       s.Letter.Name.Bn.Length > 0 ? s.Letter.Name.Bn : s.Letter.Name.En,
        SignName:            s.Sign.Name.Bn.Length   > 0 ? s.Sign.Name.Bn   : s.Sign.Name.En,
        SignGroup:           s.Sign.SignGroup,
        AudioUrl:            s.AudioUrl
    );

    private static SyllableDto ToSyllableDto(SyllableSound s) => new(
        CombinedCharacter:   s.CombinedCharacter,
        Transliteration:     s.Transliteration.En,
        TransliterationBn:   s.Transliteration.Bn,
        TransliterationText: s.TransliterationText,
        AudioUrl:            s.AudioUrl,
        Details: new SyllableDetails(
            LetterName: s.Letter.Name.Bn.Length > 0 ? s.Letter.Name.Bn : s.Letter.Name.En,
            SignName:   s.Sign.Name.Bn.Length   > 0 ? s.Sign.Name.Bn   : s.Sign.Name.En,
            SignGroup:  s.Sign.SignGroup
        )
    );
}
