using System;
using System.Text;

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Основание и целая степень (с новых строк):");
        double a = double.Parse(Console.ReadLine());
        int n = int.Parse(Console.ReadLine());
        Console.WriteLine(Power(a, n));

    }
    public static double Power(double a, int n)
    {
        if(a == 0 && n < 0)throw new ArgumentException("Ноль нельзя возвести в отрицательную степень.");
        long degree = n;
        if(degree < 0)
        {
            a = 1 / a;
            degree = - degree;
        }
        double result = 1;
        while(degree > 0)
        {
            if(degree % 2 == 1)result *= a;
            a *= a;
            degree /= 2;
        }
        return result;
    }

}


