using backend.Application.DTOs;

namespace backend.Application.Interfaces;

/// <summary>
/// Generates an AI-powered detailed explanation for an Arabic letter
/// using Microsoft Semantic Kernel backed by Azure OpenAI.
/// </summary>
public interface ILetterExplanationService
{
    /// <param name="letter">The full ArabicLetterDto (makhraj, sifaat, forms already loaded).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<LetterExplanationDto> ExplainAsync(ArabicLetterDto letter, CancellationToken ct = default);
}
