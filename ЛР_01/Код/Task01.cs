using System;
using System.Text;

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Введите четыре целых числа, каждое с новой строки:");
        int a = int.Parse(Console.ReadLine()), b = int.Parse(Console.ReadLine()), c = int.Parse(Console.ReadLine()), d = int.Parse(Console.ReadLine());
        Console.WriteLine("Минимум: " + Min(a, b, c, d));

    }
    public static int Min(int a, int b, int c, int d)
    {
        return Math.Min(Math.Min(a, b), Math.Min(c, d));
    }

}


