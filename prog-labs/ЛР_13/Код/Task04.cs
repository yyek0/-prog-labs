using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Task4
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            Console.Write("Введите текст: ");
            string text = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Текст пуст.");
            var counts = new Dictionary<char, int>();
            foreach (char raw in text)
            {
                if (!char.IsLetter(raw)) continue;
                char letter = char.ToLowerInvariant(raw);
                if (!counts.ContainsKey(letter)) counts[letter] = 0;
                counts[letter]++;
            }
            if (counts.Count == 0) throw new ArgumentException("В тексте нет букв.");
            foreach (var pair in counts.OrderBy(x => x.Key))
                Console.WriteLine(pair.Key + ": " + pair.Value);
            int least = counts.Min(x => x.Value), most = counts.Max(x => x.Value);
            Console.WriteLine("Реже всего: " + string.Join(", ", counts.Where(x => x.Value == least).Select(x => x.Key)));
            Console.WriteLine("Чаще всего: " + string.Join(", ", counts.Where(x => x.Value == most).Select(x => x.Key)));
        }
        catch (ArgumentException ex) { Console.WriteLine("Ошибка ввода: " + ex.Message); }
    }
}
