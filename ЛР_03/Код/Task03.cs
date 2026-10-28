using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class NumberArray
{
    private int[] data;
    public int Length
    {
        get
        {
            return data.Length;
        }
    }
    public NumberArray(int n)
    {
        if(n <= 0)throw new ArgumentException("Размер должен быть положительным.");
        data = new int[n];
    }
    public int this[int index]
    {
        get
        {
            return data[index];
        }
        set
        {
            data[index] = value;
        }
    }
}
class Task3
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Размер массива:");
        int n = int.Parse(Console.ReadLine());
        var a = new NumberArray(n);
        for(int i = 0; i < n; i++)
        {
            Console.WriteLine("Элемент " + i + ":");
            a[i] = int.Parse(Console.ReadLine());
        }
        Console.WriteLine("Четные элементы: " + string.Join("; ", Even(a)));

    }
    public static int[] Even(NumberArray a)
    {
        var result = new List < int > ();
        for(int i = 0; i < a.Length; i++)if(a[i] % 2 == 0)result.Add(a[i]);
        return result.ToArray();
    }

}


