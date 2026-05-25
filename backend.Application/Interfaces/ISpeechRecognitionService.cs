namespace backend.Application.Interfaces;

public interface ISpeechRecognitionService
{
    Task<string> RecognizeAsync(string audioFilePath, CancellationToken ct = default);
}
