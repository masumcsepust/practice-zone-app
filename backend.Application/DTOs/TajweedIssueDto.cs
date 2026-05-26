namespace backend.Application.DTOs;

public record TajweedIssueDto(
    string  RuleType,
    string  Word,
    string  AffectedLetter,
    string  Feedback,
    string  Severity,
    double? ActualDurationMs,
    double? ExpectedDurationMs);
