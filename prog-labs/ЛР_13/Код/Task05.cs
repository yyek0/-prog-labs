using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Task5
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Введите целые числа через пробел:");
        int[] numbers = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var groups = new Queue<int>();
        for (int i = 0; i < numbers.Length; i++)
            if (i == 0 || numbers[i] != numbers[i - 1]) groups.Enqueue(numbers[i]);
        Console.WriteLine("По одному из каждой подряд идущей группы: " + string.Join(" ", groups));
    }
}
