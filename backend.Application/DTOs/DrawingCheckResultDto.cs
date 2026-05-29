namespace backend.Application.DTOs;

/// <summary>
/// Result of the AI-powered Arabic letter drawing check.
/// </summary>
public record DrawingCheckResultDto(
    bool   IsCorrect,
    int    Score,        // 0–100
    string Feedback,
    string FeedbackBn,
    string Hint,
    string HintBn
);
