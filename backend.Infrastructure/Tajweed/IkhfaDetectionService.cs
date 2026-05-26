using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.ValueObjects;

namespace backend.Infrastructure.Tajweed;

public sealed class IkhfaDetectionService : IIkhfaDetectionService
{
    // 15 Ikhfa letters: ت ث ج د ذ ز س ش ص ض ط ظ ف ق ك
    private static readonly HashSet<char> IkhfaLetters =
    ['ت', 'ث', 'ج', 'د', 'ذ', 'ز', 'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ف', 'ق', 'ك'];

    public IReadOnlyList<TajweedIssue> Detect(
        IReadOnlyList<WordPronunciationDto> words, string referenceText)
    {
        var issues = new List<TajweedIssue>();
        var refWords = ArabicTextHelper.SplitWords(referenceText);

        // Word-boundary rule: look at adjacent pairs
        int count = Math.Min(words.Count, refWords.Length);
        for (int w = 0; w < count - 1; w++)
        {
            var rawCurrent = refWords[w];
            var rawNext    = refWords[w + 1];

            if (!ArabicTextHelper.EndsWithNoonSakinahOrTanween(rawCurrent)) continue;

            char firstNext = ArabicTextHelper.FirstLetter(rawNext);
            if (!IkhfaLetters.Contains(firstNext)) continue;

            // The ending phoneme of the current word should be partially hidden (<65 accuracy)
            var word = words[w];
            if (word.Phonemes.Count == 0) continue;

            var lastPhoneme = word.Phonemes[ArabicTextHelper.LastPhonemeIndex(word.Phonemes.Count)];
            if (lastPhoneme.AccuracyScore >= 65.0) continue;

            issues.Add(new TajweedIssue
            {
                RuleType         = "Ikhfa",
                Word             = word.Word,
                AffectedLetter   = "ن",
                Feedback         = $"Ikhfa: noon sakinah/tanween before '{firstNext}' must be partially hidden (soft nasalization).",
                Severity         = "medium",
                ActualDurationMs = null,
                ExpectedDurationMs = null
            });
        }

        return issues;
    }
}
