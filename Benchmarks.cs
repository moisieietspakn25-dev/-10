
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace OpamLab10;

internal static class Benchmarks
{
    public static double Measure(Action action)
    {
        // Прогрів
        action();

        double best = double.MaxValue;

        for (int i = 0; i < 3; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Stopwatch sw = Stopwatch.StartNew();

            action();

            sw.Stop();

            if (sw.Elapsed.TotalMilliseconds < best)
                best = sw.Elapsed.TotalMilliseconds;
        }

        return best;
    }

    public static void RunStringPitfalls(string prose)
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 1 ===");

        // Пастка 1
        foreach (int count in new[] { 10000, 100000 })
        {
            string source = new string('А', count);

            double concatTime = Measure(() =>
            {
                string result = "";

                foreach (char c in source)
                    result += c;

                GC.KeepAlive(result);
            });

            double builderTime = Measure(() =>
            {
                StringBuilder sb = new();

                foreach (char c in source)
                    sb.Append(c);

                GC.KeepAlive(sb.ToString());
            });

            Console.WriteLine(
                $"{count} символів: += {concatTime:F3} мс; " +
                $"StringBuilder = {builderTime:F3} мс"
            );
        }

        // Пастка 2
        List<string> words = GetWords(prose);

        if (words.Count < 2)
        {
            Console.WriteLine("Для порівняння потрібен текст із словами.");
        }
        else
        {
            double lowerTime = Measure(() =>
            {
                int matches = 0;

                for (int i = 0; i < 100000; i++)
                {
                    if (words[i % words.Count].ToLower() ==
                        words[(i + 1) % words.Count].ToLower())
                    {
                        matches++;
                    }
                }

                GC.KeepAlive(matches);
            });

            double ordinalTime = Measure(() =>
            {
                int matches = 0;

                for (int i = 0; i < 100000; i++)
                {
                    if (string.Equals(
                        words[i % words.Count],
                        words[(i + 1) % words.Count],
                        StringComparison.OrdinalIgnoreCase))
                    {
                        matches++;
                    }
                }

                GC.KeepAlive(matches);
            });

            Console.WriteLine(
                $"ToLower: {lowerTime:F3} мс; " +
                $"OrdinalIgnoreCase: {ordinalTime:F3} мс"
            );
        }

        // Пастка 3
        string[] sourceWords =
        {
            "ґанок", "ґудзик", "ґрунт", "ґава",
            "ґелґотіти", "ґатунок", "ґрати",
            "ґречний", "ґирлиґа", "ґлей",
            "ґніт", "ґазда", "ґвалт",
            "ґедзь", "ґешефт", "ґудзь",
            "ґлейкий", "ґуля", "ґрунтовний",
            "ґоґель-моґель"
        };

        string[] ordinal = (string[])sourceWords.Clone();
        string[] cultural = (string[])sourceWords.Clone();

        Array.Sort(ordinal, StringComparer.Ordinal);

        Array.Sort(
            cultural,
            StringComparer.Create(
                new CultureInfo("uk-UA"),
                false
            )
        );

        Console.WriteLine("Ordinal:");
        Console.WriteLine(string.Join(", ", ordinal));

        Console.WriteLine("Культурне сортування:");
        Console.WriteLine(string.Join(", ", cultural));

        Console.WriteLine(
            "Порядок відрізняється: " +
            !string.Join("|", ordinal).Equals(
                string.Join("|", cultural),
                StringComparison.Ordinal
            )
        );
    }

    public static List<string> GetWords(string text)
    {
        List<string> words = new();

        StringBuilder current = new();

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                current.Append(c);
            }
            else if (current.Length > 0)
            {
                words.Add(current.ToString());
                current.Clear();
            }
        }

        if (current.Length > 0)
            words.Add(current.ToString());

        return words;
    }
}
