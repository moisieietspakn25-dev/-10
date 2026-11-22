
using System.Text;

namespace OpamLab10;

internal static class Algorithms
{
    // Наївна нормалізація
    public static string NormalizeNaive(string input)
    {
        string text = input.ToLowerInvariant().Trim();

        while (text.Contains("  ", StringComparison.Ordinal))
        {
            text = text.Replace(
                "  ",
                " ",
                StringComparison.Ordinal
            );
        }

        StringBuilder result = new();

        int start = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            if (i == text.Length || char.IsWhiteSpace(text[i]))
            {
                if (i > start)
                {
                    result.Append(
                        text.Substring(start, i - start)
                    );

                    result.Append(' ');
                }

                start = i + 1;
            }
        }

        return result.ToString().TrimEnd();
    }

    // Швидка нормалізація за один прохід
    public static string NormalizeFast(string input)
    {
        StringBuilder result = new(input.Length);

        bool pendingSpace = false;
        bool hasContent = false;

        for (int i = 0; i < input.Length; i++)
        {
            char ch = input[i];

            if (char.IsWhiteSpace(ch))
            {
                if (hasContent)
                    pendingSpace = true;

                continue;
            }

            if (pendingSpace)
            {
                result.Append(' ');
                pendingSpace = false;
            }

            result.Append(char.ToLowerInvariant(ch));

            hasContent = true;
        }

        return result.ToString();
    }

    // Власний наївний пошук підрядка
    public static int NaiveIndexOf(
        string text,
        string pattern,
        out long comparisons)
    {
        comparisons = 0;

        if (pattern.Length == 0)
            return 0;

        if (pattern.Length > text.Length)
            return -1;

        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            int j = 0;

            while (j < pattern.Length)
            {
                comparisons++;

                if (text[i + j] != pattern[j])
                    break;

                j++;
            }

            if (j == pattern.Length)
                return i;
        }

        return -1;
    }

    // Пошук із Substring для додаткового завдання
    public static int SearchSubstring(
        string text,
        string pattern)
    {
        if (pattern.Length == 0)
            return 0;

        if (pattern.Length > text.Length)
            return -1;

        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            string part = text.Substring(i, pattern.Length);

            if (part.Equals(pattern, StringComparison.Ordinal))
                return i;
        }

        return -1;
    }

    // Порівняння без копіювання рядків
    public static int SearchByIndex(
        string text,
        string pattern,
        out long comparisons)
    {
        comparisons = 0;

        if (pattern.Length == 0)
            return 0;

        if (pattern.Length > text.Length)
            return -1;

        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            int j = 0;

            while (j < pattern.Length)
            {
                comparisons++;

                if (text[i + j] != pattern[j])
                    break;

                j++;
            }

            if (j == pattern.Length)
                return i;
        }

        return -1;
    }
}
