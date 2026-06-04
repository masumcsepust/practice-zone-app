namespace backend.Application.DTOs;

// ── Wrapper ──────────────────────────────────────────────────────────────────

public record LessonResponse<T>(string Status, T Data);

// ── Shared building blocks ────────────────────────────────────────────────────

public record LessonHeader(
    string Title,
    int    CurrentLesson,
    int    TotalLessons,
    int    ProgressPercentage
);

public record LessonConcept(
    string         TitleEn,
    string         TitleBn,
    string         DescriptionEn,
    string         DescriptionBn,
    ConceptExample Example
);

public record ConceptExample(
    string BaseText,
    string ResultText,
    string ResultType,
    string AudioUrl
);

public record ListenCompare(
    string Label,
    string AudioUrl
);

// ── Tanween lesson ─────────────────────────────────────────────────────────────

public record TanweenLessonData(
    LessonHeader              Header,
    LessonConcept             Concept,
    IReadOnlyList<TanweenType> TanweenTypes,
    IReadOnlyList<ListenCompare> ListenAndCompare
);

public record TanweenType(
    int          Id,
    TanweenLetter BaseLetter,
    TanweenLetter TargetLetter
);

public record TanweenLetter(
    string Arabic,
    string Transliteration,
    string VowelType,
    string AudioUrl
);

// ── Harakat lesson ─────────────────────────────────────────────────────────────

public record HarakatLessonData(
    LessonHeader               Header,
    LessonConcept              Concept,
    IReadOnlyList<HarakatType>  HarakatTypes,
    IReadOnlyList<ListenCompare> ListenAndCompare
);

public record HarakatType(
    int          Id,
    HarakatLetter Letter
);

public record HarakatLetter(
    string Arabic,
    string Transliteration,
    string VowelType,
    string AudioUrl
);
