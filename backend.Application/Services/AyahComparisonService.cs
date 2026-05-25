using backend.Application.Interfaces;
using backend.Domain.Entities;

namespace backend.Application.Services;

public class AyahComparisonService : IAyahComparisonService
{
    private const int CorrectThreshold = 85;

    public RecitationResult Compare(string normalizedExpected, string normalizedRecognized)
    {
        var expectedWords = normalizedExpected.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var recognizedWords = normalizedRecognized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var maxLen = Math.Max(expectedWords.Length, recognizedWords.Length);
        var wordMatches = new List<WordMatchResult>(maxLen);

        for (int i = 0; i < maxLen; i++)
        {
            var expected = i < expectedWords.Length ? expectedWords[i] : string.Empty;
            var recognized = i < recognizedWords.Length ? recognizedWords[i] : string.Empty;
            var similarity = CalculateSimilarity(expected, recognized);

            wordMatches.Add(new WordMatchResult
            {
                ExpectedWord = expected,
                RecognizedWord = recognized,
                IsCorrect = similarity >= CorrectThreshold,
                SimilarityScore = similarity
            });
        }

        var overallScore = wordMatches.Count > 0
            ? (int)wordMatches.Average(w => w.SimilarityScore)
            : 0;

        return new RecitationResult
        {
            OverallScore = overallScore,
            RecognizedText = normalizedRecognized,
            WordMatches = wordMatches
        };
    }

    private static int CalculateSimilarity(string a, string b)
    {
        if (a == b) return 100;
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0;

        int distance = LevenshteinDistance(a, b);
        int maxLen = Math.Max(a.Length, b.Length);
        return (int)Math.Round((1.0 - (double)distance / maxLen) * 100);
    }

    private static int LevenshteinDistance(string a, string b)
    {
        int m = a.Length, n = b.Length;
        var dp = new int[m + 1, n + 1];

        for (int i = 0; i <= m; i++) dp[i, 0] = i;
        for (int j = 0; j <= n; j++) dp[0, j] = j;

        for (int i = 1; i <= m; i++)
            for (int j = 1; j <= n; j++)
                dp[i, j] = a[i - 1] == b[j - 1]
                    ? dp[i - 1, j - 1]
                    : 1 + Math.Min(dp[i - 1, j - 1], Math.Min(dp[i - 1, j], dp[i, j - 1]));

        return dp[m, n];
    }
}
