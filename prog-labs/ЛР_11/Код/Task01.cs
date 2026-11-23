using System;
using System.Text;

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Func<int, int, int> product = (a, b) => a * b;
        Console.WriteLine("Таблица умножения по строкам:");
        for (int a = 1; a <= 10; a++)
        {
            for (int b = 1; b <= 10; b++) Console.Write("{0,4}", product(a, b));
            Console.WriteLine();
        }
        Console.WriteLine("Таблица умножения в столбцах:");
        for (int b = 1; b <= 10; b++)
        {
            for (int a = 1; a <= 10; a++) Console.Write("{0}×{1}={2,-3} ", a, b, product(a, b));
            Console.WriteLine();
        }
    }
}
