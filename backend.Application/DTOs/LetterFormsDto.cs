namespace backend.Application.DTOs;

public record LetterFormsDto(
    Guid   Id,
    int    Order,
    string Letter,
    string NameEnglish,
    string IsolatedForm,
    string InitialForm,
    string MedialForm,
    string FinalForm,
    bool   IsConnector
);
