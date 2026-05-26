using backend.Application.DTOs;
using backend.Domain.ValueObjects;

namespace backend.Application.Interfaces;

public interface IMaddDetectionService
{
    IReadOnlyList<TajweedIssue> Detect(IReadOnlyList<WordPronunciationDto> words, string referenceText);
}
