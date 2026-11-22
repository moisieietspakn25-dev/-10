
using System.Text;

namespace OpamLab10;

internal class Program
{
    public const int N = 17;
    public const int K = 20;

    public const int A = N % 3;
    public const int B = N % 4;
    public const int C = N % 5;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine($"N = {N}, K = {K}, a = {A}, b = {B}, c = {C}");
        Console.WriteLine("Лабораторна робота №10");
        Console.WriteLine(new string('-', 60));

        string prose = DataLoader.LoadProse();

        Console.WriteLine($"Розмір тексту: {prose.Length} символів");

        Benchmarks.RunStringPitfalls(prose);

        Task2.Run(prose);

        Task3.Run(prose);

        Task4.Run();

        ExtraBenchmark.Run();

        EdgeCases.Run();

        Console.WriteLine("\nЛабораторну роботу завершено.");
    }
}

internal static class DataLoader
{
    public static string LoadProse()
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "proza.txt"
        );

        if (!File.Exists(path))
        {
            path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "proza.txt"
            );
        }

        if (!File.Exists(path))
        {
            Console.WriteLine("ПОМИЛКА: файл Data/proza.txt не знайдено.");
            Console.WriteLine("Додайте український прозовий текст від 50000 символів.");
            return "";
        }

        return File.ReadAllText(path, Encoding.UTF8);
    }
}
