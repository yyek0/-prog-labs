using System;
using System.Text;

class Task3
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Введите два значения true или false:");
        bool x = bool.Parse(Console.ReadLine()), y = bool.Parse(Console.ReadLine());
        Console.WriteLine(Xor(x, y));

    }
    public static bool Xor(bool x, bool y)
    {
        return x!= y;
    }

}


