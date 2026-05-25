using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Persistence.Repositories;

public class PronunciationRepository
{
    private readonly AppDbContext _ctx;

    public PronunciationRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<PronunciationAssessmentResult> SaveAsync(
        int streamingSessionId,
        int ayahId,
        PronunciationResponseDto dto,
        CancellationToken ct = default)
    {
        var assessment = new PronunciationAssessmentResult
        {
            StreamingSessionId = streamingSessionId,
            AyahId             = ayahId,
            RecognizedText     = dto.RecognizedText,
            AccuracyScore      = dto.AccuracyScore,
            FluencyScore       = dto.FluencyScore,
            CompletenessScore  = dto.CompletenessScore,
            PronunciationScore = dto.PronunciationScore,
            CreatedAt          = DateTime.UtcNow,
            Words = dto.Words.Select(w => new WordPronunciationResult
            {
                Word          = w.Word,
                AccuracyScore = w.AccuracyScore,
                ErrorType     = w.ErrorType,
                IsCorrect     = w.IsCorrect,
                Phonemes = w.Phonemes.Select(p => new PhonemeResult
                {
                    Phoneme       = p.Phoneme,
                    AccuracyScore = p.AccuracyScore,
                    Duration      = p.Duration,
                    IsWeak        = p.IsWeak
                }).ToList()
            }).ToList()
        };

        await _ctx.PronunciationAssessmentResults.AddAsync(assessment, ct);
        await _ctx.SaveChangesAsync(ct);
        return assessment;
    }
}
