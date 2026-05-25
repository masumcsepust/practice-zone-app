using System.Text.Json;
using backend.Application.DTOs;
using backend.Application.Interfaces;

namespace backend.Application.Services;

public class PhonemeAnalysisService : IPhonemeAnalysisService
{
    private const double WeakThreshold    = 70.0;
    private const double CorrectThreshold = 75.0;

    public IReadOnlyList<WordPronunciationDto> ParseWords(string azureDetailedJson)
    {
        if (string.IsNullOrWhiteSpace(azureDetailedJson)) return [];

        try
        {
            using var doc  = JsonDocument.Parse(azureDetailedJson);
            var root       = doc.RootElement;

            if (!root.TryGetProperty("NBest", out var nBest) || nBest.GetArrayLength() == 0)
                return [];

            if (!nBest[0].TryGetProperty("Words", out var wordsEl))
                return [];

            var result = new List<WordPronunciationDto>();

            foreach (var wordEl in wordsEl.EnumerateArray())
            {
                var word      = GetString(wordEl, "Word");
                var offset    = GetLong(wordEl, "Offset");
                var duration  = GetLong(wordEl, "Duration");

                double wordAccuracy = 0;
                string errorType    = "None";

                if (wordEl.TryGetProperty("PronunciationAssessment", out var pa))
                {
                    wordAccuracy = GetDouble(pa, "AccuracyScore");
                    errorType    = GetString(pa, "ErrorType", "None");
                }

                var phonemes = new List<PhonemeDto>();

                if (wordEl.TryGetProperty("Phonemes", out var phonemesEl))
                {
                    foreach (var ph in phonemesEl.EnumerateArray())
                    {
                        var phoneme         = GetString(ph, "Phoneme");
                        var phonemeDuration = GetLong(ph, "Duration");
                        double phonemeScore = 0;

                        if (ph.TryGetProperty("PronunciationAssessment", out var phPa))
                            phonemeScore = GetDouble(phPa, "AccuracyScore");

                        phonemes.Add(new PhonemeDto(phoneme, phonemeScore, phonemeDuration,
                            IsWeak: phonemeScore < WeakThreshold));
                    }
                }

                var isCorrect = wordAccuracy >= CorrectThreshold && errorType == "None";

                result.Add(new WordPronunciationDto(word, wordAccuracy, isCorrect, errorType, phonemes));
            }

            return result;
        }
        catch
        {
            return [];
        }
    }

    private static string GetString(JsonElement el, string prop, string fallback = "")
        => el.TryGetProperty(prop, out var v) ? v.GetString() ?? fallback : fallback;

    private static double GetDouble(JsonElement el, string prop)
        => el.TryGetProperty(prop, out var v) ? v.GetDouble() : 0.0;

    private static long GetLong(JsonElement el, string prop)
        => el.TryGetProperty(prop, out var v) ? v.GetInt64() : 0L;
}
