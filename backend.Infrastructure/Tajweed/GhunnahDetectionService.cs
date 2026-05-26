using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.ValueObjects;

namespace backend.Infrastructure.Tajweed;

public sealed class GhunnahDetectionService : IGhunnahDetectionService
{
    // Ghunnah must be held for ~2 harakats (~300ms in practice; slightly shorter than Madd)
    private const long GhunnahMinTicks = 3_000_000L;

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

                // Ghunnah applies to ن or م with shaddah (doubled, nasalized)
                bool isGhunnah = (c == 'ن' || c == 'م')
                              && ArabicTextHelper.HasShaddah(rawWord, rawPos);

                if (!isGhunnah) continue;

                int pi = ArabicTextHelper.MapToPhonemeIndex(li, total, word.Phonemes.Count);
                var phoneme = word.Phonemes[pi];

                if (phoneme.Duration <= 0 || phoneme.Duration >= GhunnahMinTicks) continue;

                issues.Add(new TajweedIssue
                {
                    RuleType           = "Ghunnah",
                    Word               = word.Word,
                    AffectedLetter     = c.ToString(),
                    Feedback           = "Ghunnah (nasalization) must be sustained for 2 harakats.",
                    Severity           = "medium",
                    ActualDurationMs   = phoneme.Duration / 10_000.0,
                    ExpectedDurationMs = GhunnahMinTicks / 10_000.0
                });
            }
        }

        return issues;
    }
}
