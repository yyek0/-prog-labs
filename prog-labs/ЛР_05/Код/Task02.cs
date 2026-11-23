using System;
using System.Collections.Generic;
using System.Text;

static class Plane
{
    public class Flight
    {
        public int Number;
        public string From, To;
        public int FreeSeats;
        public Flight(int number, string from, string to, int freeSeats)
        { Number = number; From = from; To = to; FreeSeats = freeSeats; }
        public override string ToString()
        { return string.Format("Рейс {0}: {1}–{2}, свободных мест {3}", Number, From, To, FreeSeats); }
    }
    private static readonly List<Flight> flights = new List<Flight>();
    public static void Add(Flight flight) { flights.Add(flight); }
    public static IEnumerable<Flight> WithMoreThan(int seats)
    { foreach (Flight flight in flights) if (flight.FreeSeats > seats) yield return flight; }
}

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Plane.Add(new Plane.Flight(101, "Москва", "Казань", 8));
        Plane.Add(new Plane.Flight(102, "Казань", "Сочи", 16));
        Plane.Add(new Plane.Flight(103, "Уфа", "Пермь", 2));
        Plane.Add(new Plane.Flight(104, "Москва", "Уфа", 25));
        Plane.Add(new Plane.Flight(105, "Самара", "Москва", 10));
        Console.Write("Минимум свободных мест: ");
        int threshold = int.Parse(Console.ReadLine());
        bool found = false;
        foreach (Plane.Flight flight in Plane.WithMoreThan(threshold))
        { Console.WriteLine(flight); found = true; }
        if (!found) Console.WriteLine("Подходящих рейсов нет.");
    }
}
