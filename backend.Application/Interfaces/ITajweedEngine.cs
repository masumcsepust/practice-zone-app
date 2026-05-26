using backend.Application.DTOs;

namespace backend.Application.Interfaces;

public interface ITajweedEngine
{
    TajweedAnalysisResponseDto Analyze(
        PronunciationResponseDto pronunciation,
        string referenceText);
}
