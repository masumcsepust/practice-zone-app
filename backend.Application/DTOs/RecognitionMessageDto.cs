namespace backend.Application.DTOs;

public class RecognitionMessageDto
{
    public string Type { get; init; } = string.Empty;   // partial | final
    public string Text { get; init; } = string.Empty;
}
