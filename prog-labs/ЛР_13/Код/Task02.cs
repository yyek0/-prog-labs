using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

class P1
{
    public string Name;
    public int[] Marks;
    public P1(string name, params int[] marks) { Name = name; Marks = marks; }
    public override string ToString() { return Name + ": " + string.Join(", ", Marks); }
}
class P2
{
    public int Number;
    public string Route;
    public TimeSpan Arrival, Departure;
    public P2(int number, string route, TimeSpan arrival, TimeSpan departure)
    { Number = number; Route = route; Arrival = arrival; Departure = departure; }
    public override string ToString()
    { return string.Format("№{0} {1}, {2:hh\\:mm}–{3:hh\\:mm}", Number, Route, Arrival, Departure); }
}
class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var students = new[] {
            new P1("Иванов", 5, 5, 5, 5, 5),
            new P1("Петров", 4, 4, 5, 4, 5),
            new P1("Сидорова", 3, 4, 5, 4, 5)
        };
        var trains = new[] {
            new P2(11, "Москва–Казань", new TimeSpan(9, 0, 0), new TimeSpan(9, 15, 0)),
            new P2(22, "Казань–Уфа", new TimeSpan(10, 0, 0), new TimeSpan(10, 20, 0)),
            new P2(33, "Уфа–Пермь", new TimeSpan(11, 0, 0), new TimeSpan(11, 10, 0))
        };
        var mixed = new ArrayList();
        mixed.AddRange(students); mixed.AddRange(trains);
        Console.WriteLine("ArrayList, по три объекта каждого класса:");
        foreach (object item in mixed) Console.WriteLine(item);
        var list = new List<P1>(students);
        Console.WriteLine("List<P1>:");
        foreach (P1 item in list) Console.WriteLine(item);
        var linked = new LinkedList<P1>();
        foreach (P1 student in students) linked.AddLast(student);
        Console.WriteLine("LinkedList<P1>:");
        foreach (P1 item in linked) Console.WriteLine(item);
    }
}
