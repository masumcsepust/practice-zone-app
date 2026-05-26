using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.ValueObjects;

namespace backend.Infrastructure.Tajweed;

public sealed class QalqalahDetectionService : IQalqalahDetectionService
{
    // ق ط ب ج د — the five Qalqalah letters
    private static readonly HashSet<char> QalqalahLetters = ['ق', 'ط', 'ب', 'ج', 'د'];

    public IReadOnlyList<TajweedIssue> Detect(
        IReadOnlyList<WordPronunciationDto> words, string referenceText)
    {
        var issues = new List<TajweedIssue>();
        var refWords = ArabicTextHelper.SplitWords(referenceText);

        int count = Math.Min(words.Count, refWords.Length);
        for (int w = 0; w < count; w++)
        {
            var word = words[w];
            if (word.Phonemes.Count == 0) continue;

            var rawWord = refWords[w];
            var positions = ArabicTextHelper.GetLetterPositions(rawWord);
            int total = positions.Count;

            for (int li = 0; li < total; li++)
            {
                int rawPos = positions[li];
                char c = rawWord[rawPos];

                if (!QalqalahLetters.Contains(c)) continue;

                // Qalqalah only applies at word end or when the letter carries sukoon
                bool isWordEnd = ArabicTextHelper.IsLastLetter(li, total);
                bool hasSukoon = ArabicTextHelper.HasSukoon(rawWord, rawPos);

                if (!isWordEnd && !hasSukoon) continue;

                int pi = ArabicTextHelper.MapToPhonemeIndex(li, total, word.Phonemes.Count);
                var phoneme = word.Phonemes[pi];

                if (phoneme.AccuracyScore >= 65.0) continue;

                issues.Add(new TajweedIssue
                {
                    RuleType         = "Qalqalah",
                    Word             = word.Word,
                    AffectedLetter   = c.ToString(),
                    Feedback         = "Qalqalah letter requires a short echoing bounce sound.",
                    Severity         = phoneme.AccuracyScore < 40.0 ? "high" : "medium",
                    ActualDurationMs = null,
                    ExpectedDurationMs = null
                });
            }
        }

        return issues;
    }
}
