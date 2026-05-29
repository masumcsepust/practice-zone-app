using backend.Application.DTOs;

namespace backend.Application.Interfaces;

/// <summary>
/// Uses Azure OpenAI vision (GPT-4o) to evaluate whether a user's
/// hand-drawn image matches the target Arabic letter.
/// </summary>
public interface ILetterDrawingService
{
    Task<DrawingCheckResultDto> CheckDrawingAsync(
        ArabicLetterDto letter,
        string          imageBase64,
        CancellationToken ct = default);
}
