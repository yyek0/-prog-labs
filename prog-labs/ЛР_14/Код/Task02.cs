using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Session
{
    public string Group, Surname;
    public int[] Exams;
    public bool[] Credits;
    public int Debts { get { return Exams.Count(m => m < 3) + Credits.Count(c => !c); } }
    public double Average { get { return Exams.Average(); } }
    public string Serialize()
    {
        return Group + ";" + Surname + ";" + string.Join(",", Exams) + ";" + string.Join(",", Credits.Select(c => c ? "З" : "Н"));
    }
    public static Session Parse(string line)
    {
        string[] parts = line.Split(';');
        if (parts.Length != 4) throw new FormatException("Неверный формат записи.");
        var session = new Session {
            Group = parts[0], Surname = parts[1],
            Exams = parts[2].Split(',').Select(int.Parse).ToArray(),
            Credits = parts[3].Split(',').Select(c => c == "З" ? true : c == "Н" ? false : throwBad()).ToArray()
        };
        if (session.Exams.Length != 5 || session.Credits.Length != 5 || session.Exams.Any(m => m < 2 || m > 5))
            throw new FormatException("Нужно пять оценок и пять зачётов.");
        return session;
    }
    private static bool throwBad() { throw new FormatException("Зачёт должен быть З или Н."); }
}

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Число студентов: ");
        int count = int.Parse(Console.ReadLine());
        if (count < 0) throw new ArgumentOutOfRangeException("count");
        var records = new List<Session>();
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine("Студент " + (i + 1));
            Console.Write("Группа: "); string group = Console.ReadLine();
            Console.Write("Фамилия: "); string surname = Console.ReadLine();
            Console.Write("Пять оценок через пробел: ");
            int[] exams = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            Console.Write("Пять зачётов З или Н через пробел: ");
            bool[] credits = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s == "З" ? true : s == "Н" ? false : throwBad()).ToArray();
            if (exams.Length != 5 || credits.Length != 5 || exams.Any(m => m < 2 || m > 5))
                throw new FormatException("Нужно пять оценок от 2 до 5 и пять зачётов З/Н.");
            records.Add(new Session { Group = group, Surname = surname, Exams = exams, Credits = credits });
        }
        string path = Path.Combine(Environment.CurrentDirectory, "lab14_session.txt");
        File.WriteAllLines(path, records.Select(x => x.Serialize()).ToArray(), Encoding.UTF8);
        var fromFile = File.ReadAllLines(path, Encoding.UTF8).Select(Session.Parse).ToArray();
        Console.WriteLine("Неуспевающие:");
        foreach (Session s in fromFile.Where(x => x.Debts > 0))
            Console.WriteLine("{0}, группа {1}, долгов {2}", s.Surname, s.Group, s.Debts);
        Console.Write("Группа X: ");
        string requested = Console.ReadLine();
        var groupRecords = fromFile.Where(s => s.Group == requested).ToArray();
        if (groupRecords.Length == 0) { Console.WriteLine("Группа не найдена."); return; }
        foreach (Session s in groupRecords)
            Console.WriteLine("{0}: средний балл {1:F2}", s.Surname, s.Average);
        Console.WriteLine("Средний балл группы: {0:F2}", groupRecords.Average(s => s.Average));
    }
    private static bool throwBad() { throw new FormatException("Зачёт должен быть З или Н."); }
}
