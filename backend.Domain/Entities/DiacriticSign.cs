namespace backend.Domain.Entities;

public class DiacriticSign
{
    public Guid   Id        { get; set; }
    public string Symbol    { get; set; } = string.Empty;
    public LocalizedText Name { get; set; } = null!;
    public string SignGroup { get; set; } = string.Empty;
}
