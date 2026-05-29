namespace backend.Domain.Entities;

public class ArabicLetter
{
    public int    Id                   { get; set; }
    public int    Order                { get; set; }
    public string Letter               { get; set; } = string.Empty;
    public string NameEnglish          { get; set; } = string.Empty;
    public string NameArabic           { get; set; } = string.Empty;
    public string NameBangla           { get; set; } = string.Empty;
    public string Transliteration      { get; set; } = string.Empty;
    public string MakhrajType          { get; set; } = string.Empty;
    public string MakhrajDescription   { get; set; } = string.Empty;
    public string MakhrajDescriptionBn { get; set; } = string.Empty;
    public string Sifaat               { get; set; } = string.Empty;
    public string ExampleWordArabic    { get; set; } = string.Empty;
    public string ExampleWord          { get; set; } = string.Empty;
    public string ExampleWordBn        { get; set; } = string.Empty;
    // Positional forms
    public string IsolatedForm         { get; set; } = string.Empty;
    public string InitialForm          { get; set; } = string.Empty;
    public string MedialForm           { get; set; } = string.Empty;
    public string FinalForm            { get; set; } = string.Empty;
    public bool   IsConnector          { get; set; } = true;
}
