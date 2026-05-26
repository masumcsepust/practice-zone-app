namespace backend.Infrastructure.Tajweed;

internal static class ArabicTextHelper
{
    private static readonly HashSet<char> Diacritics =
    [
        'ً', // Fathatan (tanween fath)
        'ٌ', // Dammatan (tanween damm)
        'ٍ', // Kasratan (tanween kasr)
        'َ', // Fatha
        'ُ', // Damma
        'ِ', // Kasra
        'ّ', // Shaddah
        'ْ', // Sukoon
        'ٓ', 'ٔ', 'ٕ', 'ٖ', 'ٗ', '٘',
        'ٙ', 'ٚ', 'ٛ', 'ٜ', 'ٝ', 'ٞ', 'ٟ',
        'ٰ', // Superscript alef (alef khanjariyya)
        'ۖ', 'ۗ', 'ۘ', 'ۙ', 'ۚ', 'ۛ', 'ۜ',
        '۟', '۠', 'ۡ', 'ۢ', 'ۣ', 'ۤ',
        'ۧ', 'ۨ', '۪', '۫', '۬', 'ۭ',
    ];

    internal static bool IsDiacritic(char c) => Diacritics.Contains(c);

    internal static bool IsArabicLetter(char c) =>
        c >= '؀' && c <= 'ۿ' && !Diacritics.Contains(c);

    // Raw character positions of base letters (non-diacritic) in the word
    internal static List<int> GetLetterPositions(string rawWord)
    {
        var positions = new List<int>();
        for (int i = 0; i < rawWord.Length; i++)
            if (IsArabicLetter(rawWord[i]))
                positions.Add(i);
        return positions;
    }

    // Map a letter index to a phoneme index by proportional distribution
    internal static int MapToPhonemeIndex(int letterIdx, int totalLetters, int phonemeCount)
    {
        if (phonemeCount == 0 || totalLetters == 0) return 0;
        if (totalLetters == 1) return 0;
        int idx = (int)Math.Round((double)letterIdx / (totalLetters - 1) * (phonemeCount - 1));
        return Math.Clamp(idx, 0, phonemeCount - 1);
    }

    // Diacritic immediately preceding the character at rawPos (0 = none)
    internal static char PrecedingDiacritic(string rawWord, int rawPos)
    {
        if (rawPos > 0 && IsDiacritic(rawWord[rawPos - 1]))
            return rawWord[rawPos - 1];
        return '\0';
    }

    // Diacritic immediately following the character at rawPos (0 = none)
    internal static char FollowingDiacritic(string rawWord, int rawPos)
    {
        if (rawPos < rawWord.Length - 1 && IsDiacritic(rawWord[rawPos + 1]))
            return rawWord[rawPos + 1];
        return '\0';
    }

    internal static bool HasShaddah(string rawWord, int rawPos) =>
        FollowingDiacritic(rawWord, rawPos) == 'ّ';

    internal static bool HasSukoon(string rawWord, int rawPos) =>
        FollowingDiacritic(rawWord, rawPos) == 'ْ';

    internal static bool IsLastLetter(int letterIdx, int totalLetters) =>
        letterIdx == totalLetters - 1;

    // Split Arabic text into words
    internal static string[] SplitWords(string text) =>
        text.Split([' ', '‌', '‍', ' '], StringSplitOptions.RemoveEmptyEntries);

    // First base letter of a raw word (0 = none)
    internal static char FirstLetter(string rawWord)
    {
        foreach (char c in rawWord)
            if (IsArabicLetter(c)) return c;
        return '\0';
    }

    // True if word ends with noon sakinah (ن + sukoon) or any tanween
    internal static bool EndsWithNoonSakinahOrTanween(string rawWord)
    {
        if (rawWord.Length == 0) return false;

        // Tanween is always the last character
        char last = rawWord[^1];
        if (last is 'ً' or 'ٌ' or 'ٍ') return true;

        // Noon sakinah: find last base letter, check it is ن with sukoon
        for (int i = rawWord.Length - 1; i >= 0; i--)
        {
            if (IsDiacritic(rawWord[i])) continue;
            return rawWord[i] == 'ن' && HasSukoon(rawWord, i);
        }
        return false;
    }

    // Index of the last letter in the letter positions list whose phoneme we want for boundary checks
    internal static int LastPhonemeIndex(int phonemeCount) => Math.Max(0, phonemeCount - 1);
}
