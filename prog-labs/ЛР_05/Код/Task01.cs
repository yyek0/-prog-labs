using System;
using System.Text;

static class ArrayMaximum
{
    public static int Max(int[] array)
    {
        if (array == null || array.Length == 0) throw new ArgumentException("Массив пуст.");
        return MaxFrom(array, 0);
    }
    private static int MaxFrom(int[] array, int index)
    {
        if (index == array.Length - 1) return array[index];
        return Math.Max(array[index], MaxFrom(array, index + 1));
    }
}

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        int[][] arrays = {
            new[] { 1, 7, 3 }, new[] { -8, -2, -6 },
            new[] { 9, 4, 9, 2 }, new[] { 0, 0, 0 }, new[] { 17, -2, 5 }
        };
        for (int i = 0; i < arrays.Length; i++)
            Console.WriteLine("Массив {0}: максимум {1}", i + 1, ArrayMaximum.Max(arrays[i]));
    }
}
