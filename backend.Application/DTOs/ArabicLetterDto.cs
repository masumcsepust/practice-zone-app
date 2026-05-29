namespace backend.Application.DTOs;

public record ArabicLetterDto(
    int      Id,
    int      Order,
    string   Letter,
    string   NameEnglish,
    string   NameArabic,
    string   NameBangla,
    string   Transliteration,
    string   MakhrajType,
    string   MakhrajDescription,
    string   MakhrajDescriptionBn,
    string[] Sifaat,
    string   ExampleWordArabic,
    string   ExampleWord,
    string   ExampleWordBn,
    string   IsolatedForm,
    string   InitialForm,
    string   MedialForm,
    string   FinalForm,
    bool     IsConnector,
    string   AudioUrl
);
