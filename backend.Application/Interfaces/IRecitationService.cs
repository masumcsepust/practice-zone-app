using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface IRecitationService
{
    Task<RecitationResultDto> ProcessAsync(RecognizeRecitationRequest request, CancellationToken ct = default);
}
