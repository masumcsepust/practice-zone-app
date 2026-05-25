using backend.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace backend.Infrastructure.Storage;

public class LocalAudioStorageService : IAudioStorageService
{
    private readonly string _basePath;

    public LocalAudioStorageService(IConfiguration config)
    {
        _basePath = config["AudioStorage:BasePath"] ?? "AudioUploads";
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(Stream audioStream, string fileName, CancellationToken ct = default)
    {
        var ext = Path.GetExtension(fileName);
        var storedFileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(_basePath, storedFileName);

        await using var fileStream = File.Create(filePath);
        await audioStream.CopyToAsync(fileStream, ct);

        return filePath;
    }
}
