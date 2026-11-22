
namespace OpamLab10;

internal static class EdgeCases
{
    public static void Run()
    {
        Console.WriteLine("\n=== КРАЙОВІ ВИПАДКИ ===");

        CheckString(
            "Порожній рядок",
            Algorithms.NormalizeFast(""),
            ""
        );

        CheckString(
            "Один символ",
            Algorithms.NormalizeFast("А"),
            "а"
        );

        CheckString(
            "Однакові символи",
            Algorithms.NormalizeFast("аааа"),
            "аааа"
        );

        CheckString(
            "Зайві пробіли",
            Algorithms.NormalizeFast("  Привіт   світ  "),
            "привіт світ"
        );

        CheckNumber(
            "Шаблон довший за текст",
            Algorithms.NaiveIndexOf(
                "abc",
                "abcdef",
                out _
            ),
            -1
        );

        CheckNumber(
            "Порожній шаблон",
            Algorithms.NaiveIndexOf(
                "abc",
                "",
                out _
            ),
            0
        );

        CheckNumber(
            "Нуль елементів",
            0,
            0
        );

        CheckNumber(
            "Один елемент",
            1,
            1
        );
    }

    private static void CheckString(
        string name,
        string actual,
        string expected)
    {
        bool success = string.Equals(
            actual,
            expected,
            StringComparison.Ordinal
        );

        Console.WriteLine(
            $"{name}: {(success ? "OK" : "ПОМИЛКА")}"
        );
    }

    private static void CheckNumber(
        string name,
        int actual,
        int expected)
    {
        Console.WriteLine(
            $"{name}: {(actual == expected ? "OK" : "ПОМИЛКА")}"
        );
    }
}
