using backend.Application.DTOs;

namespace backend.Application.Interfaces;

/// <summary>
/// One-shot HTTP-based letter pronunciation check.
/// Accepts raw audio bytes and returns a structured bilingual result.
/// </summary>
public interface ILetterPronunciationService
{
    Task<LetterPronunciationResultDto> CheckAsync(
        ArabicLetterDto   letter,
        byte[]            audioBytes,
        string            mimeType  = "audio/wav",
        CancellationToken ct        = default);
}
