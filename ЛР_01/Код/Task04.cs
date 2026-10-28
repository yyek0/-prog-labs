using System;
using System.Text;

class Task4
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Введите три числа 0 или 1:");
        bool x = Read(), y = Read(), z = Read();
        Console.WriteLine(Election(x, y, z)?1:0);

    }
    public static bool Election(bool x, bool y, bool z)
    {
        return (x&&y)||(x&&z)||(y&&z);
    }
    static bool Read()
    {
        string s = Console.ReadLine();
        if(s!= "0"&&s!= "1")throw new FormatException("Допустимы только 0 и 1.");
        return s == "1";
    }

}


