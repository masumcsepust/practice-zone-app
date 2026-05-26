using backend.Application.DTOs;
using backend.Application.Interfaces;

namespace backend.Infrastructure.Tajweed;

public sealed class TajweedEngine : ITajweedEngine
{
    private readonly IMaddDetectionService     _madd;
    private readonly IGhunnahDetectionService  _ghunnah;
    private readonly IQalqalahDetectionService _qalqalah;
    private readonly IIkhfaDetectionService    _ikhfa;
    private readonly IIdghamDetectionService   _idgham;

    public TajweedEngine(
        IMaddDetectionService     madd,
        IGhunnahDetectionService  ghunnah,
        IQalqalahDetectionService qalqalah,
        IIkhfaDetectionService    ikhfa,
        IIdghamDetectionService   idgham)
    {
        _madd     = madd;
        _ghunnah  = ghunnah;
        _qalqalah = qalqalah;
        _ikhfa    = ikhfa;
        _idgham   = idgham;
    }

    public TajweedAnalysisResponseDto Analyze(
        PronunciationResponseDto pronunciation, string referenceText)
    {
        var words = pronunciation.Words;

        var allIssues = new List<Domain.ValueObjects.TajweedIssue>();
        allIssues.AddRange(_madd.Detect(words, referenceText));
        allIssues.AddRange(_ghunnah.Detect(words, referenceText));
        allIssues.AddRange(_qalqalah.Detect(words, referenceText));
        allIssues.AddRange(_ikhfa.Detect(words, referenceText));
        allIssues.AddRange(_idgham.Detect(words, referenceText));

        double deduction = allIssues.Sum(i => i.Severity switch
        {
            "high"   => 15.0,
            "medium" => 8.0,
            _        => 3.0
        });

        double tajweedScore = Math.Max(0.0, 100.0 - deduction);

        var issueDtos = allIssues
            .Select(i => new TajweedIssueDto(
                i.RuleType, i.Word, i.AffectedLetter,
                i.Feedback, i.Severity,
                i.ActualDurationMs, i.ExpectedDurationMs))
            .ToList();

        return new TajweedAnalysisResponseDto
        {
            RecognizedText     = pronunciation.RecognizedText,
            PronunciationScore = pronunciation.PronunciationScore,
            AccuracyScore      = pronunciation.AccuracyScore,
            FluencyScore       = pronunciation.FluencyScore,
            CompletenessScore  = pronunciation.CompletenessScore,
            TajweedScore       = tajweedScore,
            Words              = pronunciation.Words,
            WeakPhonemes       = pronunciation.WeakPhonemes,
            TajweedIssues      = issueDtos
        };
    }
}
