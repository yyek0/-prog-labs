using System;
using System.Text;

class Task3
{
    delegate void Transform(int a);
    static void Modul(int a) { Console.WriteLine("Модуль: " + Math.Abs((long)a)); }
    static void Kub(int a) { Console.WriteLine("Куб: " + ((long)a * a * a)); }

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Целое число: ");
        int a = int.Parse(Console.ReadLine());
        Transform both = Modul;
        both += Kub; // Обе функции будут вызваны последовательно.
        both(a);
    }
}
