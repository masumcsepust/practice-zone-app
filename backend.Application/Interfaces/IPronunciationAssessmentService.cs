using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface IPronunciationAssessmentService
{
    Task StartSessionAsync(
        string connectionId,
        string referenceText,
        Func<PronunciationResponseDto, Task> onResult,
        Func<string, Task>? onError = null,
        CancellationToken ct = default);

    Task WriteAudioAsync(string connectionId, byte[] chunk);
    Task CloseInputAsync(string connectionId);
    Task StopSessionAsync(string connectionId);
}
