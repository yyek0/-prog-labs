using System;
using System.Text;

abstract class Konstructor
{
    public abstract void Stroka();
    public abstract void Massiv();
}

class Konstructor1 : Konstructor
{
    private readonly string text;
    private readonly char target;
    private readonly int[] numbers;
    public Konstructor1(string text, char target, int[] numbers)
    { this.text = text; this.target = target; this.numbers = numbers; }
    public override void Stroka()
    {
        int count = 0;
        foreach (char c in text) if (c == target) count++;
        Console.WriteLine("Символ '{0}' встречается {1} раз.", target, count);
    }
    public override void Massiv()
    {
        int sum = 0;
        foreach (int n in numbers) if (n < 0) sum += n;
        Console.WriteLine("Сумма отрицательных элементов: " + sum);
    }
}

class Konstructor2 : Konstructor
{
    private readonly string word;
    private readonly int[,] matrix;
    public Konstructor2(string word, int[,] matrix)
    { this.word = word; this.matrix = matrix; }
    public override void Stroka()
    {
        const string vowels = "аеёиоуыэюя";
        int run = 0;
        bool okay = word.Length > 0;
        bool previousVowel = false;
        foreach (char letter in word.ToLowerInvariant())
        {
            if (letter < 'а' || letter > 'я' && letter != 'ё') { okay = false; break; }
            bool isVowel = vowels.IndexOf(letter) >= 0;
            run = run > 0 && isVowel == previousVowel ? run + 1 : 1;
            if (run > 2) okay = false;
            previousVowel = isVowel;
        }
        Console.WriteLine(okay ? "Слово благозвучное" : "Слово неблагозвучное");
    }
    public override void Massiv()
    {
        bool found = false;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            bool hasOne = false;
            for (int j = 0; j < matrix.GetLength(1); j++) if (matrix[i, j] == 1) hasOne = true;
            if (!hasOne) { Console.WriteLine("Строка без единиц: " + (i + 1)); found = true; }
        }
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            bool hasOne = false;
            for (int i = 0; i < matrix.GetLength(0); i++) if (matrix[i, j] == 1) hasOne = true;
            if (!hasOne) { Console.WriteLine("Столбец без единиц: " + (j + 1)); found = true; }
        }
        if (!found) Console.WriteLine("Строк и столбцов без единиц нет.");
    }
}

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Строка: "); string text = Console.ReadLine();
        Console.Write("Искомый символ: "); string symbol = Console.ReadLine();
        if (string.IsNullOrEmpty(symbol)) throw new ArgumentException("Символ не введён.");
        Console.Write("Слово для проверки: "); string word = Console.ReadLine();
        Konstructor[] examples = {
            new Konstructor1(text, symbol[0], new[] { -3, 2, -5, 1 }),
            new Konstructor2(word, new[,] { { 0, 1, 0 }, { 0, 0, 0 }, { 0, 1, 0 } })
        };
        foreach (Konstructor example in examples)
        { example.Stroka(); example.Massiv(); }
    }
}
