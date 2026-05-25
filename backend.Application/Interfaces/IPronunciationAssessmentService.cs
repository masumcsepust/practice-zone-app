using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface IPronunciationAssessmentService
{
    Task StartSessionAsync(
        string connectionId,
        string referenceText,
        Func<PronunciationResponseDto, Task> onResult,
        CancellationToken ct = default);

    Task WriteAudioAsync(string connectionId, byte[] chunk);
    Task StopSessionAsync(string connectionId);
}
