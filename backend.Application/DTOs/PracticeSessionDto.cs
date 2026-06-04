using System.Text.Json.Serialization;

namespace backend.Application.DTOs;

public record PracticeSessionResponse(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("data")]   PracticeSessionData Data
);

public record PracticeSessionData(
    [property: JsonPropertyName("lesson_id")]      Guid   LessonId,
    [property: JsonPropertyName("title")]          string Title,
    [property: JsonPropertyName("sequence_order")] int    SequenceOrder,
    [property: JsonPropertyName("practice_items")] IReadOnlyList<PracticeItemSessionDto> PracticeItems
);

public record PracticeItemSessionDto(
    [property: JsonPropertyName("id")]                  Guid         Id,
    [property: JsonPropertyName("instruction")]         string       Instruction,
    [property: JsonPropertyName("target_syllable")]     SyllableDto  TargetSyllable,
    [property: JsonPropertyName("compare_with_syllable")] SyllableDto? CompareWithSyllable,
    [property: JsonPropertyName("success_tip")]         string       SuccessTip
);

public record SyllableDto(
    [property: JsonPropertyName("combined_character")]  string           CombinedCharacter,
    [property: JsonPropertyName("transliteration")]     string           Transliteration,
    [property: JsonPropertyName("transliteration_bn")]  string           TransliterationBn,
    [property: JsonPropertyName("audio_url")]           string           AudioUrl,
    [property: JsonPropertyName("details")]             SyllableDetails  Details
);

public record SyllableDetails(
    [property: JsonPropertyName("letter_name")] string LetterName,
    [property: JsonPropertyName("sign_name")]   string SignName,
    [property: JsonPropertyName("sign_group")]  string SignGroup
);
