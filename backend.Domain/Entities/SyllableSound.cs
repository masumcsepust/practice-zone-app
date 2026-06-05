namespace backend.Domain.Entities;

public class SyllableSound
{
    public Guid   Id                { get; set; }
    public Guid   LetterId          { get; set; }
    public Guid   SignId             { get; set; }
    public string CombinedCharacter  { get; set; } = string.Empty;
    public LocalizedText Transliteration { get; set; } = null!;
    public string TransliterationText { get;} = string.Empty;
    public string AudioUrl           { get; set; } = string.Empty;

    public ArabicLetter  Letter { get; set; } = null!;
    public DiacriticSign Sign   { get; set; } = null!;
}
