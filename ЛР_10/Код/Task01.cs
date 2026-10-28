using System;
using System.Text;

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        for(int n = 5; n <= 20; n++)Console.WriteLine("{0} -> {1}", n, Transform(n));

    }
    public static int Transform(int n)
    {
        return n % 5 == 0?n * 5:n - 5;
    }

}


