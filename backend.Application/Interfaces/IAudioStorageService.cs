namespace backend.Application.Interfaces;

public interface IAudioStorageService
{
    Task<string> SaveAsync(Stream audioStream, string fileName, CancellationToken ct = default);
}
