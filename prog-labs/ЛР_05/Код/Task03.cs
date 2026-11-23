using System;
using System.Text;

class Data
{
    public static int Count { get; private set; }
    public DateTime Birthday { get; private set; }

    public Data(int day, int month, int year)
    {
        Birthday = new DateTime(year, month, day);
        if (Birthday > DateTime.Today) throw new ArgumentException("Дата рождения в будущем.");
        Count++;
    }

    public static void Vozrast(Data person, out int years, out int days)
    {
        DateTime today = DateTime.Today;
        years = today.Year - person.Birthday.Year;
        DateTime anniversary = person.Birthday.AddYears(years);
        if (anniversary > today) { years--; anniversary = person.Birthday.AddYears(years); }
        days = (today - anniversary).Days;
    }
}

class Task3
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var people = new[] { new Data(1, 1, 2000), new Data(23, 11, 2005), new Data(29, 2, 2004) };
        foreach (Data person in people)
        {
            int years, days;
            Data.Vozrast(person, out years, out days);
            Console.WriteLine("{0:dd.MM.yyyy}: {1} лет и {2} дней после дня рождения", person.Birthday, years, days);
        }
        Console.WriteLine("Создано экземпляров: " + Data.Count);
    }
}
