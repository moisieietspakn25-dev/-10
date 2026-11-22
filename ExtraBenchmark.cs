
using System.Diagnostics;

namespace OpamLab10;

internal static class ExtraBenchmark
{
    public static void Run()
    {
        Console.WriteLine("\n=== ДОДАТКОВЕ ЗАВДАННЯ. c = 2 ===");

        string text = new string('П', 10000);
        string pattern = new string('П', 1099) + "Р";

        long allocatedBytes = 0;
        long comparisons = 0;

        double substringTime = Benchmarks.Measure(() =>
        {
            GC.KeepAlive(
                Algorithms.SearchSubstring(text, pattern)
            );
        });

        long before = GC.GetAllocatedBytesForCurrentThread();

        int substringResult =
            Algorithms.SearchSubstring(text, pattern);

        allocatedBytes =
            GC.GetAllocatedBytesForCurrentThread() - before;

        double indexTime = Benchmarks.Measure(() =>
        {
            GC.KeepAlive(
                Algorithms.SearchByIndex(
                    text,
                    pattern,
                    out _
                )
            );
        });

        int indexResult = Algorithms.SearchByIndex(
            text,
            pattern,
            out comparisons
        );

        Console.WriteLine(
            $"Substring: {substringTime:F3} мс"
        );

        Console.WriteLine(
            $"Виділено пам'яті: {allocatedBytes:N0} байт"
        );

        Console.WriteLine(
            $"Порівняння за індексами: {indexTime:F3} мс"
        );

        Console.WriteLine(
            $"Кількість порівнянь: {comparisons:N0}"
        );

        Console.WriteLine(
            $"Результати: {substringResult}, {indexResult}"
        );

        Console.WriteLine(
            "Висновок: індексний пошук не створює проміжних " +
            "рядків для кожної позиції."
        );
    }
}
