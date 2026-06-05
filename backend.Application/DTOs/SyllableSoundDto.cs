namespace backend.Application.DTOs;

public record SyllableSoundDto(
    Guid   Id,
    Guid   LetterId,
    string LetterCharacter,
    string LetterNameEn,
    Guid   SignId,
    string SignSymbol,
    string SignNameEn,
    string CombinedCharacter,
    string TransliterationEn,
    string TransliterationBn,
    string AudioUrl
);

public record CreateSyllableSoundDto(
    Guid   LetterId,
    Guid   SignId,
    string CombinedCharacter,
    string TransliterationEn,
    string TransliterationBn,
    string AudioUrl
);

public record UpdateSyllableSoundDto(
    string CombinedCharacter,
    string TransliterationEn,
    string TransliterationBn,
    string AudioUrl
);
