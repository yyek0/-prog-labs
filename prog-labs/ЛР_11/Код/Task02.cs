using System;
using System.Text;

class Task2
{
    delegate double Stepen(int a, int n);

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Основание a: ");
        int a = int.Parse(Console.ReadLine());
        Stepen power = (x, n) => Math.Pow(x, n);
        Console.WriteLine("Таблица степеней числа " + a);
        for (int n = 0; n <= 10; n++)
            Console.WriteLine("{0}^{1} = {2}", a, n, power(a, n));
    }
}
