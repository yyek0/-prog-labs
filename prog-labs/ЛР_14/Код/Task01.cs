using System;
using System.IO;
using System.Linq;
using System.Text;

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Количество чисел в файле: ");
        int count = int.Parse(Console.ReadLine());
        if (count <= 0) throw new ArgumentOutOfRangeException("count");
        Console.Write("Ненулевой делитель A: ");
        int a = int.Parse(Console.ReadLine());
        if (a == 0) throw new ArgumentException("На ноль делить нельзя.");

        // Для целых чисел интервал варианта 7 [-19,35; 6,02] даёт [-19; 6].
        var random = new Random();
        int[] numbers = Enumerable.Range(0, count).Select(i => random.Next(-19, 7)).ToArray();
        string path = Path.Combine(Environment.CurrentDirectory, "lab14_numbers.txt");
        File.WriteAllLines(path, numbers.Select(n => n.ToString()).ToArray());
        int[] read = File.ReadAllLines(path).Select(int.Parse).ToArray();
        Console.WriteLine("Файл: " + path);
        for (int i = 0; i < read.Length; i++) Console.WriteLine("№{0}: {1}", i + 1, read[i]);

        long product = 1;
        int matching = 0;
        for (int i = 0; i < read.Length; i++)
            if ((i + 1) % 2 == 0 && read[i] % a == 0)
            {
                product = checked(product * read[i]);
                matching++;
            }
        Console.WriteLine(matching == 0 ? "Подходящих чисел нет." : "Произведение: " + product);
    }
}
