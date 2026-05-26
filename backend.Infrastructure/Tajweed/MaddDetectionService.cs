using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.ValueObjects;

namespace backend.Infrastructure.Tajweed;

public sealed class MaddDetectionService : IMaddDetectionService
{
    // Minimum 2 harakats in 100-ns ticks (1 harakat ≈ 1,700,000 ticks ≈ 170ms)
    private const long MaddMinTicks = 3_400_000L;

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

                // Madd: elongation letter preceded by its corresponding short vowel
                bool isMadd = c switch
                {
                    'ا' => ArabicTextHelper.PrecedingDiacritic(rawWord, rawPos) == 'َ', // alef ← fatha
                    'و' => ArabicTextHelper.PrecedingDiacritic(rawWord, rawPos) == 'ُ', // waw ← damma
                    'ي' => ArabicTextHelper.PrecedingDiacritic(rawWord, rawPos) == 'ِ', // ya ← kasra
                    _ => false
                };

                if (!isMadd) continue;

                int pi = ArabicTextHelper.MapToPhonemeIndex(li, total, word.Phonemes.Count);
                var phoneme = word.Phonemes[pi];

                // Only flag if there is timing data and the duration is too short
                if (phoneme.Duration <= 0 || phoneme.Duration >= MaddMinTicks) continue;

                double ratio = (double)phoneme.Duration / MaddMinTicks;
                issues.Add(new TajweedIssue
                {
                    RuleType           = "Madd",
                    Word               = word.Word,
                    AffectedLetter     = c.ToString(),
                    Feedback           = "Madd letter must be elongated for at least 2 harakats (~340ms).",
                    Severity           = ratio < 0.5 ? "high" : "medium",
                    ActualDurationMs   = phoneme.Duration / 10_000.0,
                    ExpectedDurationMs = MaddMinTicks / 10_000.0
                });
            }
        }

        return issues;
    }
}
