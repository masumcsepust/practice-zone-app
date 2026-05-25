using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;

namespace backend.Application.Services;

public class RecitationService : IRecitationService
{
    private readonly ISpeechRecognitionService _speech;
    private readonly IArabicTextNormalizer _normalizer;
    private readonly IAyahComparisonService _comparison;
    private readonly IAudioStorageService _audioStorage;
    private readonly IAyahRepository _ayahRepo;
    private readonly IRecitationSessionRepository _sessionRepo;

    public RecitationService(
        ISpeechRecognitionService speech,
        IArabicTextNormalizer normalizer,
        IAyahComparisonService comparison,
        IAudioStorageService audioStorage,
        IAyahRepository ayahRepo,
        IRecitationSessionRepository sessionRepo)
    {
        _speech = speech;
        _normalizer = normalizer;
        _comparison = comparison;
        _audioStorage = audioStorage;
        _ayahRepo = ayahRepo;
        _sessionRepo = sessionRepo;
    }

    public async Task<RecitationResultDto> ProcessAsync(RecognizeRecitationRequest request, CancellationToken ct = default)
    {
        var ayah = await _ayahRepo.GetByIdAsync(request.AyahId, ct)
            ?? throw new KeyNotFoundException($"Ayah with id {request.AyahId} was not found.");

        var startedAt = DateTime.UtcNow;

        // Save audio to disk first, then pass the path to Azure Speech (handles WAV headers correctly)
        var audioFilePath = await _audioStorage.SaveAsync(request.AudioStream, request.FileName, ct);

        var recognizedRaw = await _speech.RecognizeAsync(audioFilePath, ct);

        var normalizedExpected = _normalizer.Normalize(ayah.ArabicText);
        var normalizedRecognized = _normalizer.Normalize(recognizedRaw);

        var result = _comparison.Compare(normalizedExpected, normalizedRecognized);

        var session = new RecitationSession
        {
            UserId = request.UserId,
            AyahId = request.AyahId,
            RecognizedText = recognizedRaw,
            NormalizedRecognizedText = normalizedRecognized,
            OverallScore = result.OverallScore,
            AudioFilePath = audioFilePath,
            StartedAt = startedAt,
            CompletedAt = DateTime.UtcNow,
            Status = "Completed"
        };

        await _sessionRepo.AddAsync(session, ct);
        await _sessionRepo.SaveChangesAsync(ct);

        return new RecitationResultDto
        {
            Score = result.OverallScore,
            RecognizedText = recognizedRaw,
            Matches = result.WordMatches.Select(w => new WordMatchDto
            {
                ExpectedWord = w.ExpectedWord,
                RecognizedWord = w.RecognizedWord,
                IsCorrect = w.IsCorrect,
                SimilarityScore = w.SimilarityScore
            }).ToList()
        };
    }
}
