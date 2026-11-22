
namespace OpamLab10;

internal struct Job
{
    public int Start;
    public int End;

    public Job(int start, int end)
    {
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"[{Start}; {End}]";
    }
}

internal static class Task4
{
    public static void Run()
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 4. ЖАДІБНИЙ АЛГОРИТМ ===");

        Random random = new Random(Program.N);

        int optimalCount = 0;
        double worstGap = 0;

        for (int test = 0; test < 100; test++)
        {
            int count = random.Next(0, 16);

            Job[] jobs = new Job[count];

            for (int i = 0; i < count; i++)
            {
                int start = random.Next(0, 2 * Program.K + 11);
                int duration = random.Next(1, 10);

                jobs[i] = new Job(start, start + duration);
            }

            int greedy = Greedy(jobs);
            int brute = BruteForce(jobs);

            if (greedy == brute)
            {
                optimalCount++;
            }
            else if (brute > 0)
            {
                double gap =
                    (brute - greedy) * 100.0 / brute;

                if (gap > worstGap)
                    worstGap = gap;
            }
        }

        Console.WriteLine($"Успішних тестів: {optimalCount}/100");
        Console.WriteLine($"Найгірше відставання: {worstGap:F2}%");

        Console.WriteLine("\nКонтрприклад: правило початку");

        Job[] wrongStart =
        {
            new Job(0, 10),
            new Job(1, 2),
            new Job(2, 3)
        };

        ShowWrongExample(wrongStart, "За найранішим початком");

        Console.WriteLine("\nКонтрприклад: правило найкоротшої заявки");

        Job[] wrongShort =
        {
            new Job(0, 3),
            new Job(3, 6),
            new Job(2, 4)
        };

        ShowWrongExample(wrongShort, "За найкоротшою тривалістю");

        // Вимірювання повного перебору
        Job[] data15 = GenerateJobs(15, 150);
        Job[] data20 = GenerateJobs(20, 200);

        double time15 = Benchmarks.Measure(
            () => GC.KeepAlive(BruteForce(data15))
        );

        double time20 = Benchmarks.Measure(
            () => GC.KeepAlive(BruteForce(data20))
        );

        Console.WriteLine("\nЧас повного перебору:");
        Console.WriteLine($"15 заявок: {time15:F3} мс");
        Console.WriteLine($"20 заявок: {time20:F3} мс");
    }

    // Жадібний алгоритм
    private static int Greedy(Job[] jobs)
    {
        Job[] sorted = (Job[])jobs.Clone();

        Array.Sort(sorted, (x, y) =>
        {
            int result = x.End.CompareTo(y.End);

            if (result == 0)
                result = x.Start.CompareTo(y.Start);

            return result;
        });

        int count = 0;
        int lastEnd = int.MinValue;

        foreach (Job job in sorted)
        {
            if (job.Start >= lastEnd)
            {
                count++;
                lastEnd = job.End;
            }
        }

        return count;
    }

    // Повний перебір
    private static int BruteForce(Job[] jobs)
    {
        Job[] sorted = (Job[])jobs.Clone();

        Array.Sort(sorted, (x, y) =>
            x.Start.CompareTo(y.Start));

        int best = 0;

        Search(sorted, 0, int.MinValue, 0, ref best);

        return best;
    }

    private static void Search(
        Job[] jobs,
        int index,
        int lastEnd,
        int selected,
        ref int best)
    {
        if (index == jobs.Length)
        {
            if (selected > best)
                best = selected;

            return;
        }

        // Не брати заявку
        Search(
            jobs,
            index + 1,
            lastEnd,
            selected,
            ref best
        );

        // Брати заявку
        if (jobs[index].Start >= lastEnd)
        {
            Search(
                jobs,
                index + 1,
                jobs[index].End,
                selected + 1,
                ref best
            );
        }
    }

    private static Job[] GenerateJobs(int count, int seed)
    {
        Random random = new Random(seed);

        Job[] jobs = new Job[count];

        for (int i = 0; i < count; i++)
        {
            int start = random.Next(0, 60);
            int end = start + random.Next(1, 12);

            jobs[i] = new Job(start, end);
        }

        return jobs;
    }

    private static int WrongEarliestStart(Job[] jobs)
    {
        Job[] sorted = (Job[])jobs.Clone();

        Array.Sort(sorted, (x, y) =>
            x.Start.CompareTo(y.Start));

        int count = 0;
        int lastEnd = int.MinValue;

        foreach (Job job in sorted)
        {
            if (job.Start >= lastEnd)
            {
                count++;
                lastEnd = job.End;
            }
        }

        return count;
    }

    private static int WrongShortest(Job[] jobs)
    {
        Job[] sorted = (Job[])jobs.Clone();

        Array.Sort(sorted, (x, y) =>
        {
            int xDuration = x.End - x.Start;
            int yDuration = y.End - y.Start;

            return xDuration.CompareTo(yDuration);
        });

        List<Job> selected = new();

        foreach (Job job in sorted)
        {
            bool compatible = true;

            foreach (Job existing in selected)
            {
                if (job.Start < existing.End &&
                    job.End > existing.Start)
                {
                    compatible = false;
                    break;
                }
            }

            if (compatible)
                selected.Add(job);
        }

        return selected.Count;
    }

    private static void ShowWrongExample(
        Job[] jobs,
        string title)
    {
        Console.WriteLine(title);

        foreach (Job job in jobs)
            Console.Write(job + " ");

        Console.WriteLine();

        Console.WriteLine(
            $"Неправильне правило: {WrongValue(jobs, title)}"
        );

        Console.WriteLine(
            $"Оптимум повного перебору: {BruteForce(jobs)}"
        );
    }

    private static int WrongValue(Job[] jobs, string title)
    {
        if (title.Contains("початком", StringComparison.Ordinal))
            return WrongEarliestStart(jobs);

        return WrongShortest(jobs);
    }
}
