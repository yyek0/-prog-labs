using System;
using System.Text;

class Task4
{
    delegate void PrintNumbers(int lower, int upper, int requiredSum);

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Сумма цифр B: ");
        int b = int.Parse(Console.ReadLine());
        if (b < 1) throw new ArgumentOutOfRangeException("b");

        PrintNumbers print = delegate(int lower, int upper, int requiredSum)
        {
            Console.WriteLine("Числа от " + lower + " до " + upper + ":");
            bool found = false;
            for (int number = lower; number <= upper; number++)
            {
                int value = number, sum = 0;
                while (value > 0) { sum += value % 10; value /= 10; }
                if (sum != requiredSum || number % 2 != 0) continue;
                Console.Write(number + " ");
                found = true;
            }
            Console.WriteLine(found ? "" : "подходящих чисел нет");
        };

        print(10, 99, b);
        print(100, 999, b);
        print(1000, 9999, b);
    }
}
