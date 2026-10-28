using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Task6
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Путь к текстовому файлу UTF - 8:");
        foreach(string word in Words(File.ReadAllText(Console.ReadLine(), Encoding.UTF8)))Console.WriteLine(word);

    }
    public static string[] Words(string text)
    {
        var c = StringComparer.Create(CultureInfo.GetCultureInfo("ru - RU"), true);
        var words = new SortedSet < string > (c);
        foreach(Match m in Regex.Matches(text, @"\p{L} + (?:[- ’']\p{L} +) * "))words.Add(m.Value);
        return words.ToArray();
    }

}


