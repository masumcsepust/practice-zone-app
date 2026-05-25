using backend.Domain.Entities;

namespace backend.Application.Interfaces;

public interface IAyahComparisonService
{
    RecitationResult Compare(string normalizedExpected, string normalizedRecognized);
}
