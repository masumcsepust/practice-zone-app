namespace backend.Application.DTOs;

/// <summary>
/// Lightweight DTO containing only the four positional forms of an Arabic letter.
/// Used by GET /api/arabic-letters/forms and GET /api/arabic-letters/{id}/forms.
/// </summary>
public record LetterFormsDto(
    int    Id,
    int    Order,
    string Letter,
    string NameEnglish,
    string IsolatedForm,   // standalone:  ا
    string InitialForm,    // word-start:  بـ
    string MedialForm,     // word-middle: ـبـ
    string FinalForm,      // word-end:    ـب
    bool   IsConnector
);
