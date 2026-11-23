using System;
using System.Text;

interface ITrainInfo
{
    string Describe();
}

struct TRAIN : ITrainInfo
{
    public string Destination;
    public int Number;

    public TRAIN(string destination, int number)
    {
        Destination = destination;
        Number = number;
    }

    public string Describe()
    {
        return "Поезд №" + Number + ", пункт назначения: " + Destination;
    }
}

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var train = new TRAIN("Казань", 17);
        ITrainInfo boxed = train; // Значение структуры скопировано в объект интерфейса.
        train.Number = 21;
        Console.WriteLine("Исходная структура: " + train.Describe());
        Console.WriteLine("После упаковки: " + boxed.Describe());
        TRAIN unboxed = (TRAIN)boxed;
        Console.WriteLine("После распаковки: " + unboxed.Describe());
    }
}
