using System;
using System.Text;

class Task1
{
    static long Modul(int a) { return Math.Abs((long)a); }
    static long Kub(int a) { return (long)a * a * a; }

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Целое число: ");
        int a = int.Parse(Console.ReadLine());
        Console.WriteLine("Модуль: " + Modul(a));
        Console.WriteLine("Куб: " + Kub(a));
    }
}
