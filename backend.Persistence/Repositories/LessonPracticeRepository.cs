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
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.TargetSyllable).ThenInclude(s => s.Letter)
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.TargetSyllable).ThenInclude(s => s.Sign)
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.CompareWithSyllable!).ThenInclude(s => s.Letter)
             .Include(l => l.PracticeItems)
                 .ThenInclude(p => p.CompareWithSyllable!).ThenInclude(s => s.Sign);

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

    private static SyllableDto ToSyllableDto(SyllableSound s) => new(
        CombinedCharacter:  s.CombinedCharacter,
        Transliteration:    s.Transliteration.En,
        TransliterationBn:  s.Transliteration.Bn,
        AudioUrl:           s.AudioUrl,
        Details: new SyllableDetails(
            LetterName: s.Letter.Name.Bn.Length > 0 ? s.Letter.Name.Bn : s.Letter.Name.En,
            SignName:   s.Sign.Name.Bn.Length   > 0 ? s.Sign.Name.Bn   : s.Sign.Name.En,
            SignGroup:  s.Sign.SignGroup
        )
    );
}
