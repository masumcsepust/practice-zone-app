namespace backend.Application.DTOs;

public class RecognizeRecitationRequest
{
    public int AyahId { get; init; }
    public int UserId { get; init; } = 1;
    public Stream AudioStream { get; init; } = Stream.Null;
    public string FileName { get; init; } = string.Empty;
}
