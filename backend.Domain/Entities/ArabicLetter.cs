namespace backend.Domain.Entities;

public class ArabicLetter
{
    public Guid Id { get; set; }
    public int SequenceOrder { get; set; }
    public string Character { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public LocalizedText Name { get; set; } = null!;
    public LocalizedText Transliteration { get; set; } = null!;
    public string MakhrajType { get; set; } = string.Empty;
    public LocalizedText MakhrajDescription { get; set; } = null!;
    public string Sifaat { get; set; } = string.Empty;
    public string ExampleWordArabic { get; set; } = string.Empty;
    public LocalizedText ExampleWordMeaning { get; set; } = null!;
    public string IsolatedForm { get; set; } = string.Empty;
    public string InitialForm { get; set; } = string.Empty;
    public string MedialForm { get; set; } = string.Empty;
    public string FinalForm { get; set; } = string.Empty;
    public bool IsConnector { get; set; } = true;

    public ICollection<LetterHarakatItem> HarakatItems { get; set; } = [];
}
