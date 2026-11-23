using PSystem;

namespace PSystem
{
    using System;
    using System.Linq;
    using System.Text;

    public static class PConsole
    {
        public static void Write(string value) { Console.Write(value); }
        public static void WriteLine(string value) { Console.WriteLine(value); }
        public static string ReadLine() { return Console.ReadLine(); }
        public static int ReadInt() { return int.Parse(Console.ReadLine()); }
        public static void WriteFormat(string format, object value) { Console.WriteLine(format, value); }
        public static void Utf8() { Console.OutputEncoding = Encoding.UTF8; }
    }

    public static class PMath
    {
        public static long Abs(int x) { return Math.Abs((long)x); }
        public static int Max(int a, int b) { return Math.Max(a, b); }
        public static int Min(int a, int b) { return Math.Min(a, b); }
        public static long Square(int x) { return (long)x * x; }
        public static long Cube(int x) { return (long)x * x * x; }
    }

    public class PText
    {
        private readonly string value;
        public PText(string value) { this.value = value ?? ""; }
        public int Length() { return value.Length; }
        public string Upper() { return value.ToUpperInvariant(); }
        public string Lower() { return value.ToLowerInvariant(); }
        public string Reverse() { return new string(value.Reverse().ToArray()); }
        public bool Contains(string fragment) { return value.Contains(fragment); }
    }

    public class PNumbers
    {
        private readonly int[] values;
        public PNumbers(params int[] values) { this.values = values; }
        public int Sum() { return values.Sum(); }
        public double Average() { return values.Average(); }
        public int Max() { return values.Max(); }
        public int Min() { return values.Min(); }
        public int[] Sorted() { return values.OrderBy(x => x).ToArray(); }
    }

    public class PClock
    {
        private readonly DateTime value;
        public PClock(DateTime value) { this.value = value; }
        public int Year() { return value.Year; }
        public int Month() { return value.Month; }
        public int Day() { return value.Day; }
        public string ShortDate() { return value.ToString("dd.MM.yyyy"); }
        public DayOfWeek Weekday() { return value.DayOfWeek; }
    }

    public static class PDateFactory
    {
        public static PClock Example() { return new PClock(new DateTime(2026, 11, 23)); }
    }
}

class Task1
{
    public static void Main()
    {
        PConsole.Utf8();
        PConsole.Write("Введите слово: ");
        string input = PConsole.ReadLine();
        PConsole.Write("Введите целое число: ");
        int number = PConsole.ReadInt();
        PConsole.WriteLine("Проверка пяти классов пространства PSystem");
        PConsole.WriteFormat("Введено число: {0}", number);

        PConsole.WriteLine("Модуль: " + PMath.Abs(number));
        PConsole.WriteLine("Максимум: " + PMath.Max(number, 3));
        PConsole.WriteLine("Минимум: " + PMath.Min(number, 3));
        PConsole.WriteLine("Квадрат: " + PMath.Square(number));
        PConsole.WriteLine("Куб: " + PMath.Cube(number));

        var text = new PText(input);
        PConsole.WriteLine("Длина: " + text.Length());
        PConsole.WriteLine("Верхний регистр: " + text.Upper());
        PConsole.WriteLine("Нижний регистр: " + text.Lower());
        PConsole.WriteLine("Наоборот: " + text.Reverse());
        PConsole.WriteLine("Содержит букву а: " + text.Contains("а"));

        var numbers = new PNumbers(4, 1, number, 2);
        PConsole.WriteLine("Сумма: " + numbers.Sum());
        PConsole.WriteLine("Среднее: " + numbers.Average());
        PConsole.WriteLine("Максимум: " + numbers.Max());
        PConsole.WriteLine("Минимум: " + numbers.Min());
        PConsole.WriteLine("Сортировка: " + string.Join(", ", numbers.Sorted()));

        var clock = PDateFactory.Example();
        PConsole.WriteLine("Год: " + clock.Year());
        PConsole.WriteLine("Месяц: " + clock.Month());
        PConsole.WriteLine("День: " + clock.Day());
        PConsole.WriteLine("Дата: " + clock.ShortDate());
        PConsole.WriteLine("День недели: " + clock.Weekday());
    }
}
