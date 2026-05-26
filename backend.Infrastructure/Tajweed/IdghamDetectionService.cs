using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.ValueObjects;

namespace backend.Infrastructure.Tajweed;

public sealed class IdghamDetectionService : IIdghamDetectionService
{
    // يرملون — the six Idgham letters
    private static readonly HashSet<char> IdghamLetters = ['ي', 'ر', 'م', 'ل', 'و', 'ن'];

    // Noon that is still distinctly pronounced (not merged) will have duration above this threshold
    private const long IdghamMaxTicks = 3_400_000L;

    public IReadOnlyList<TajweedIssue> Detect(
        IReadOnlyList<WordPronunciationDto> words, string referenceText)
    {
        var issues = new List<TajweedIssue>();
        var refWords = ArabicTextHelper.SplitWords(referenceText);

        int count = Math.Min(words.Count, refWords.Length);
        for (int w = 0; w < count - 1; w++)
        {
            var rawCurrent = refWords[w];
            var rawNext    = refWords[w + 1];

            if (!ArabicTextHelper.EndsWithNoonSakinahOrTanween(rawCurrent)) continue;

            char firstNext = ArabicTextHelper.FirstLetter(rawNext);
            if (!IdghamLetters.Contains(firstNext)) continue;

            var word = words[w];
            if (word.Phonemes.Count == 0) continue;

            var lastPhoneme = word.Phonemes[ArabicTextHelper.LastPhonemeIndex(word.Phonemes.Count)];

            // Idgham error: noon is audible (duration too long = not merged into next letter)
            if (lastPhoneme.Duration <= 0 || lastPhoneme.Duration <= IdghamMaxTicks) continue;

            issues.Add(new TajweedIssue
            {
                RuleType           = "Idgham",
                Word               = word.Word,
                AffectedLetter     = "ن",
                Feedback           = $"Idgham: noon sakinah/tanween before '{firstNext}' must be merged (not pronounced distinctly).",
                Severity           = "medium",
                ActualDurationMs   = lastPhoneme.Duration / 10_000.0,
                ExpectedDurationMs = null
            });
        }

        return issues;
    }
}
