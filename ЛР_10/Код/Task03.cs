using System;
using System.Text;

class Task3
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        int[][] arrays =
        {
            new[]
            {
                1, 3, 2
            }
            , new[]
            {
                - 9, - 2, - 5
            }
            , new[]
            {
                7
            }
            , new[]
            {
                0, 0, 0
            }
            , new[]
            {
                15, 4, 19, 2
            }
        };
        foreach(var a in arrays)Console.WriteLine("[{0}] -> {1}", string.Join("; ", a), Max(a));

    }
    public static int Max(int[] a, int index = 0)
    {
        if(a == null||a.Length == 0)throw new ArgumentException("Массив пуст.");
        if(index < 0||index >= a.Length)throw new ArgumentOutOfRangeException("index");
        if(index == a.Length - 1)return a[index];
        return Math.Max(a[index], Max(a, index + 1));
    }

}


