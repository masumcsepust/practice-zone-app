namespace backend.Application.Interfaces;

public interface IArabicLetterTtsService
{
    Task<byte[]> SynthesizeAsync(string arabicText, CancellationToken ct = default);
}
