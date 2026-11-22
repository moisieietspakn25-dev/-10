
namespace OpamLab10;

internal static class Task2
{
    public static void Run(string prose)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 2. НОРМАЛІЗАЦІЯ ===");

        if (prose.Length < 50000)
        {
            Console.WriteLine("ПОМИЛКА: для завдання потрібен текст від 50000 символів.");
            return;
        }

        int[] sizes = { 1000, 10000, 50000 };

        foreach (int size in sizes)
        {
            string input = prose.Substring(0, size);

            string naive = Algorithms.NormalizeNaive(input);
            string fast = Algorithms.NormalizeFast(input);

            bool equal = string.Equals(
                naive,
                fast,
                StringComparison.Ordinal
            );

            double naiveTime = Benchmarks.Measure(
                () => GC.KeepAlive(Algorithms.NormalizeNaive(input))
            );

            double fastTime = Benchmarks.Measure(
                () => GC.KeepAlive(Algorithms.NormalizeFast(input))
            );

            double ratio = fastTime > 0
                ? naiveTime / fastTime
                : 0;

            Console.WriteLine(
                $"{size} символів | " +
                $"Наївно: {naiveTime:F3} мс | " +
                $"Швидко: {fastTime:F3} мс | " +
                $"Різниця: {ratio:F2}x | " +
                $"Результати однакові: {equal}"
            );

            if (!equal)
                throw new Exception("Алгоритми дали різні результати!");
        }
    }
}
