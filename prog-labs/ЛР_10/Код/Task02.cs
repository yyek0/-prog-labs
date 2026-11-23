using System;
using System.Text;

class Task2
{
    static int DigitSum(int number)
    {
        int sum = 0;
        while (number > 0) { sum += number % 10; number /= 10; }
        return sum;
    }

    static void PrintMatching(int lower, int upper, int b)
    {
        Console.WriteLine("Числа от " + lower + " до " + upper + ":");
        bool found = false;
        for (int number = lower; number <= upper; number++)
            if (DigitSum(number) == b && number % 2 == 0)
            {
                Console.Write(number + " ");
                found = true;
            }
        Console.WriteLine(found ? "" : "подходящих чисел нет");
    }

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Сумма цифр B: ");
        int b = int.Parse(Console.ReadLine());
        if (b < 1) throw new ArgumentOutOfRangeException("b");
        PrintMatching(10, 99, b);
        PrintMatching(100, 999, b);
        PrintMatching(1000, 9999, b);
    }
}
