using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface IPronunciationAssessmentService
{
    // ── Streaming (Quran recitation) ─────────────────────────────────────
    Task StartSessionAsync(
        string connectionId,
        string referenceText,
        Func<PronunciationResponseDto, Task> onResult,
        Func<string, Task>? onError = null,
        CancellationToken ct = default);

    Task WriteAudioAsync(string connectionId, byte[] chunk);
    Task CloseInputAsync(string connectionId);
    Task StopSessionAsync(string connectionId);

    // ── One-shot (letter practice) ────────────────────────────────────────
    /// <summary>
    /// Runs a single Pronunciation Assessment on a complete audio buffer.
    /// Returns null when no speech was detected.
    /// </summary>
    Task<PronunciationResponseDto?> AssessOnceAsync(
        string            referenceText,
        byte[]            audioBytes,
        string            mimeType,
        CancellationToken ct = default);
}
