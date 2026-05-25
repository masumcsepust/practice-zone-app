using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface IPhonemeAnalysisService
{
    IReadOnlyList<WordPronunciationDto> ParseWords(string azureDetailedJson);
}
