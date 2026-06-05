using System.Text.Json.Serialization;

namespace backend.Application.DTOs;

public record PracticeStepDto(
    [property: JsonPropertyName("id")]           int            Id,
    [property: JsonPropertyName("title")]        string         Title,
    [property: JsonPropertyName("lessonNum")]    int            LessonNum,
    [property: JsonPropertyName("progress")]     int            Progress,
    [property: JsonPropertyName("tips")]         string         Tips,
    [property: JsonPropertyName("left")]         StepSyllableDto Left,
    [property: JsonPropertyName("right")]        StepSyllableDto? Right,
    [property: JsonPropertyName("rememberText")] string         RememberText,
    [property: JsonPropertyName("totalLessons")] int            TotalLessons,
    [property: JsonPropertyName("totalSteps")]   int            TotalSteps
);

public record StepSyllableDto(
    [property: JsonPropertyName("arabic")]        string Arabic,
    [property: JsonPropertyName("translit")]      string Translit,
    [property: JsonPropertyName("bengali")]       string Bengali,
    [property: JsonPropertyName("prompt")]        string Prompt,
    [property: JsonPropertyName("letterBengali")] string LetterBengali,
    [property: JsonPropertyName("signName")]      string SignName,
    [property: JsonPropertyName("signGroup")]     string SignGroup,
    [property: JsonPropertyName("audioUrl")]      string AudioUrl
);
