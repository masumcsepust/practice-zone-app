namespace backend.Application.DTOs;

/// <summary>
/// Response returned by POST /api/arabic-letters/{id}/check-pronunciation.
/// Designed to match the Android PronunciationResult model directly.
/// </summary>
public record LetterPronunciationResultDto(
    bool   IsCorrect,
    double Score,          // overall pronunciation score 0-100
    double AccuracyScore,  // phoneme-level accuracy 0-100
    string RecognizedText, // what the speech engine transcribed
    string Feedback,       // English feedback
    string FeedbackBn,     // Bangla feedback
    string MakhrajHint     // Bangla makhraj articulation hint
);
