namespace backend.Domain.ValueObjects;

public class LetterTimingResult
{
    public char   ArabicLetter       { get; set; }
    public int    RawPositionInWord  { get; set; }   // index in original diacritised text
    public string Phoneme            { get; set; } = string.Empty;
    public double AccuracyScore      { get; set; }
    public long   DurationTicks      { get; set; }   // 100ns ticks from Azure
    public double DurationMs         => DurationTicks / 10_000.0;
    public int    WordIndex          { get; set; }
    public int    PhonemeIndex       { get; set; }
    public string ContainingWord     { get; set; } = string.Empty;
}
