using System;
using System.Text;

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string[] a =
        {
            "Раз, два, три", "Москва, Казань", "a, b, c", "Без запятых", ", начало, конец, "
        };
        foreach(string s in a)Console.WriteLine(s + " -> " + Replace(s));

    }
    public static string Replace(string text)
    {
        return text.Replace(',', ';');
    }

}



