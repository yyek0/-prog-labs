using System;
using System.Text;

class Task5
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Число сторон (3, 4 или 5):");
        int n = int.Parse(Console.ReadLine());
        if(n < 3||n > 5)throw new ArgumentException("Нужно от 3 до 5 сторон.");
        int[] a = new int[n];
        for(int i = 0; i < n; i++)
        {
            Console.WriteLine("Сторона " + (i + 1));
            a[i] = int.Parse(Console.ReadLine());
        }
        Console.WriteLine("Периметр: " + Perimetr(a));

    }
    public static int Perimetr(params int[] sides)
    {
        if(sides == null||sides.Length < 3||sides.Length > 5)throw new ArgumentException("Нужно от 3 до 5 сторон.");
        int sum = 0, max = 0;
        foreach(int s in sides)
        {
            if(s <= 0)throw new ArgumentException("Стороны должны быть положительными.");
            sum = checked(sum + s);
            max = Math.Max(max, s);
        }
        if((long)max >= (long)sum - max)throw new ArgumentException("Такой многоугольник не существует.");
        return sum;
    }

}


