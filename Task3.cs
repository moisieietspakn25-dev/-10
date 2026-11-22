
namespace OpamLab10;

internal static class Task3
{
    public static void Run(string prose)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 3. ПОШУК ПІДРЯДКА ===");

        if (prose.Length < 50000)
        {
            Console.WriteLine("Потрібно додати прозовий текст від 50000 символів.");
            return;
        }

        string text = prose.Substring(0, 50000);

        List<string> words = Benchmarks.GetWords(text);

        string existingWord = words.Count > 0
            ? words[words.Count - 1]
            : "Україна";

        string absentWord = "НЕІСНУЮЧИЙ_ШАБЛОН_123456789";

        // N = 17, K = 20
        // Літера K = П
        // K % 33 + 1 = 21 = Р
        // 900 + 10 * 20 = 1100
        string repeatedText = new string('П', 10000);

        string personalPattern =
            new string('П', 1099) + "Р";

        TestSearch(
            "Проза: слово з тексту",
            text,
            existingWord
        );

        TestSearch(
            "Проза: відсутній шаблон",
            text,
            absentWord
        );

        TestSearch(
            "Повтори: персональний шаблон",
            repeatedText,
            personalPattern
        );
    }

    private static void TestSearch(
        string name,
        string text,
        string pattern)
    {
        long comparisons = 0;
        int ownResult = -1;

        double ownTime = Benchmarks.Measure(() =>
        {
            ownResult = Algorithms.NaiveIndexOf(
                text,
                pattern,
                out comparisons
            );
        });

        // Лічильник окремо від заміру
        ownResult = Algorithms.NaiveIndexOf(
            text,
            pattern,
            out comparisons
        );

        int builtInResult = -1;

        double builtInTime = Benchmarks.Measure(() =>
        {
            builtInResult = text.IndexOf(
                pattern,
                StringComparison.Ordinal
            );
        });

        Console.WriteLine($"\n{name}");

        Console.WriteLine($"Текст: {text.Length} символів");
        Console.WriteLine($"Шаблон: {pattern.Length} символів");
        Console.WriteLine($"Позиція: {ownResult}");
        Console.WriteLine($"Порівнянь: {comparisons:N0}");
        Console.WriteLine($"Власний пошук: {ownTime:F3} мс");
        Console.WriteLine($"string.IndexOf: {builtInTime:F3} мс");

        if (ownResult != builtInResult)
            Console.WriteLine("ПОМИЛКА: результати не збігаються!");
    }
}
