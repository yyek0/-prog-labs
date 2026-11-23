using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Student
{
    public string Name;
    public int[] Marks;
    public Student(string name, params int[] marks)
    {
        if (marks.Length != 5 || marks.Any(m => m < 2 || m > 5))
            throw new ArgumentException("Нужны пять оценок от 2 до 5.");
        Name = name;
        Marks = marks;
    }
    public string Scholarship
    {
        get
        {
            if (Marks.All(m => m == 5)) return "повышенная";
            if (Marks.All(m => m >= 4)) return "обычная";
            return "нет";
        }
    }
    public override string ToString()
    {
        return Name + ": " + string.Join(", ", Marks) + "; стипендия: " + Scholarship;
    }
}

class Task1
{
    static void Print(IEnumerable<Student> students, string surname)
    {
        foreach (string group in new[] { "повышенная", "обычная", "нет" })
            foreach (Student student in students.Where(s => s.Scholarship == group))
                Console.WriteLine(student);
        var found = students.Where(s => s.Name.Split(' ')[0].Equals(surname, StringComparison.OrdinalIgnoreCase));
        foreach (Student student in found) Console.WriteLine("Найден: " + student);
    }

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var oldList = new ArrayList {
            new Student("Иванов Иван", 5, 5, 5, 5, 5),
            new Student("Петров Пётр", 4, 5, 4, 5, 4),
            new Student("Сидорова Анна", 5, 3, 4, 5, 4)
        };
        var typedList = new List<Student>();
        foreach (Student student in oldList) typedList.Add(student);
        Console.WriteLine("ArrayList:");
        Print(oldList.Cast<Student>(), "Иванов");
        Console.WriteLine("List<Student>:");
        Console.Write("Введите фамилию для поиска: ");
        Print(typedList, Console.ReadLine());
    }
}
