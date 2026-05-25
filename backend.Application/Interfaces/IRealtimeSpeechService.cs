namespace backend.Application.Interfaces;

public interface IRealtimeSpeechService
{
    Task StartSessionAsync(string connectionId,
        Func<string, string, Task> onResult,
        CancellationToken ct = default);

    Task WriteAudioAsync(string connectionId, byte[] chunk);

    Task StopSessionAsync(string connectionId);
}
