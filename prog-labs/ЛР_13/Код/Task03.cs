using System;
using System.Collections.Generic;
using System.Text;

class Task3
{
    // Вариант 7: (10,34 + 5,11) / (7,05 − 3,19) · (18,2 + 3).
    static double FromStack()
    {
        var stack = new Stack<double>();
        stack.Push(10.34); stack.Push(5.11);
        double numerator = stack.Pop() + stack.Pop();
        stack.Push(7.05); stack.Push(3.19);
        double second = stack.Pop(), first = stack.Pop();
        double denominator = first - second;
        stack.Push(18.2); stack.Push(3);
        double factor = stack.Pop() + stack.Pop();
        return numerator / denominator * factor;
    }

    static double FromQueue()
    {
        var queue = new Queue<double>();
        queue.Enqueue(10.34); queue.Enqueue(5.11);
        double numerator = queue.Dequeue() + queue.Dequeue();
        queue.Enqueue(7.05); queue.Enqueue(3.19);
        double denominator = queue.Dequeue() - queue.Dequeue();
        queue.Enqueue(18.2); queue.Enqueue(3);
        double factor = queue.Dequeue() + queue.Dequeue();
        return numerator / denominator * factor;
    }

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Стек: " + FromStack());
        Console.WriteLine("Очередь: " + FromQueue());
    }
}
