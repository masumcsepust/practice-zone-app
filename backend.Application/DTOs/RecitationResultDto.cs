namespace backend.Application.DTOs;

public class RecitationResultDto
{
    public int Score { get; init; }
    public string RecognizedText { get; init; } = string.Empty;
    public List<WordMatchDto> Matches { get; init; } = [];
}
