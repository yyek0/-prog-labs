using System;
using System.Text;

class Task3
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Целое число: ");
        int a = int.Parse(Console.ReadLine());
        Action<int> transforms = x => Console.WriteLine("Модуль: " + Math.Abs((long)x));
        transforms += x => Console.WriteLine("Куб: " + ((long)x * x * x));
        transforms(a);

        Console.Write("Сумма цифр B: ");
        int b = int.Parse(Console.ReadLine());
        if (b < 1) throw new ArgumentOutOfRangeException("b");
        Action<int, int> matching = (lower, upper) =>
        {
            Console.WriteLine("Подходящие числа от " + lower + " до " + upper + ":");
            bool found = false;
            for (int number = lower; number <= upper; number++)
            {
                int sum = 0, value = number;
                while (value > 0) { sum += value % 10; value /= 10; }
                if (sum != b || number % 2 != 0) continue;
                Console.Write(number + " ");
                found = true;
            }
            Console.WriteLine(found ? "" : "подходящих чисел нет");
        };
        matching(10, 99);
        matching(100, 999);
        matching(1000, 9999);
    }
}
