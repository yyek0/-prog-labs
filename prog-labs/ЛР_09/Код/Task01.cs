using System;
using System.Globalization;
using System.Text;

struct TRAIN
{
    public string Destination;
    public int Number;
    public TimeSpan Departure;

    public TRAIN(string destination, int number, TimeSpan departure)
    {
        Destination = destination;
        Number = number;
        Departure = departure;
    }

    public override string ToString()
    {
        return string.Format("{0}: поезд №{1}, отправление {2:hh\\:mm}",
            Destination, Number, Departure);
    }
}

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var trains = new TRAIN[8];
        for (int i = 0; i < trains.Length; i++)
        {
            Console.WriteLine("Поезд " + (i + 1));
            Console.Write("Пункт назначения: ");
            string destination = Console.ReadLine();
            Console.Write("Номер: ");
            int number = int.Parse(Console.ReadLine());
            Console.Write("Время отправления (чч:мм): ");
            TimeSpan departure = TimeSpan.ParseExact(Console.ReadLine(), @"hh\:mm", CultureInfo.InvariantCulture);
            trains[i] = new TRAIN(destination, number, departure);
        }

        Array.Sort(trains, delegate(TRAIN a, TRAIN b)
        {
            return StringComparer.CurrentCultureIgnoreCase.Compare(a.Destination, b.Destination);
        });
        Console.WriteLine("Поезда по алфавиту:");
        foreach (TRAIN train in trains) Console.WriteLine(train);

        Console.Write("Показать поезда после времени (чч:мм): ");
        TimeSpan time = TimeSpan.ParseExact(Console.ReadLine(), @"hh\:mm", CultureInfo.InvariantCulture);
        bool found = false;
        foreach (TRAIN train in trains)
            if (train.Departure > time)
            {
                Console.WriteLine(train);
                found = true;
            }
        if (!found) Console.WriteLine("Таких поездов нет.");
    }
}
