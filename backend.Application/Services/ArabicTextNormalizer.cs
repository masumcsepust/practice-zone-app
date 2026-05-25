using System.Text.RegularExpressions;
using backend.Application.Interfaces;

namespace backend.Application.Services;

public partial class ArabicTextNormalizer : IArabicTextNormalizer
{
    [GeneratedRegex(@"[ً-ٰٟ]", RegexOptions.Compiled)]
    private static partial Regex TashkeelRegex();

    public string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        text = TashkeelRegex().Replace(text, string.Empty);

        text = text
            .Replace('أ', 'ا')   // أ → ا
            .Replace('إ', 'ا')   // إ → ا
            .Replace('آ', 'ا')   // آ → ا
            .Replace('ٱ', 'ا')   // ٱ → ا
            .Replace('ة', 'ه')   // ة → ه
            .Replace('ى', 'ي')   // ى → ي
            .Replace('ئ', 'ي')   // ئ → ي
            .Replace('ؤ', 'و')   // ؤ → و
            .Replace('ـ', ' ');       // ـ tatweel removed

        return text.Trim();
    }
}
