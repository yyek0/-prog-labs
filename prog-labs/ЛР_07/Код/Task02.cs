using System;
using System.Text;

abstract class Vehicle
{
    public double X { get; set; }
    public double Y { get; set; }
    public decimal Price { get; set; }
    public double Speed { get; set; }
    public int Year { get; set; }

    protected Vehicle(double x, double y, decimal price, double speed, int year)
    { X = x; Y = y; Price = price; Speed = speed; Year = year; }

    public virtual void Move(double hours) { X += Speed * hours; }
    public virtual string Info()
    {
        return string.Format("{0}: ({1}; {2}), цена {3}, скорость {4}, год {5}",
            GetType().Name, X, Y, Price, Speed, Year);
    }
    public override bool Equals(object obj)
    {
        var other = obj as Vehicle;
        return other != null && GetType() == other.GetType() &&
            X == other.X && Y == other.Y && Price == other.Price &&
            Speed == other.Speed && Year == other.Year;
    }
    public override int GetHashCode()
    { return X.GetHashCode() ^ Y.GetHashCode() ^ Price.GetHashCode() ^ Speed.GetHashCode() ^ Year; }
}

class Plane : Vehicle
{
    public double Altitude { get; set; }
    public int Passengers { get; set; }
    public Plane(double x, double y, decimal price, double speed, int year, double altitude, int passengers)
        : base(x, y, price, speed, year) { Altitude = altitude; Passengers = passengers; }
    public override void Move(double hours) { base.Move(hours); Y += Altitude / 1000.0; }
    public override string Info() { return base.Info() + ", высота " + Altitude + ", пассажиры " + Passengers; }
    public override bool Equals(object obj)
    {
        var other = obj as Plane;
        return other != null && base.Equals(other) && Altitude == other.Altitude && Passengers == other.Passengers;
    }
    public override int GetHashCode() { return base.GetHashCode() ^ Altitude.GetHashCode() ^ Passengers; }
}

class Car : Vehicle
{
    public string Model { get; set; }
    public Car(double x, double y, decimal price, double speed, int year, string model)
        : base(x, y, price, speed, year) { Model = model; }
    public override string Info() { return base.Info() + ", модель " + Model; }
    public override bool Equals(object obj)
    {
        var other = obj as Car;
        return other != null && base.Equals(other) && Model == other.Model;
    }
    public override int GetHashCode() { return base.GetHashCode() ^ (Model ?? "").GetHashCode(); }
}

class Ship : Vehicle
{
    public int Passengers { get; set; }
    public string Port { get; set; }
    public Ship(double x, double y, decimal price, double speed, int year, int passengers, string port)
        : base(x, y, price, speed, year) { Passengers = passengers; Port = port; }
    public override string Info() { return base.Info() + ", пассажиры " + Passengers + ", порт " + Port; }
    public override bool Equals(object obj)
    {
        var other = obj as Ship;
        return other != null && base.Equals(other) && Passengers == other.Passengers && Port == other.Port;
    }
    public override int GetHashCode() { return base.GetHashCode() ^ Passengers ^ (Port ?? "").GetHashCode(); }
}

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Vehicle[] vehicles = {
            new Plane(0, 0, 1000000m, 800, 2020, 10000, 180),
            new Car(0, 0, 150000m, 90, 2022, "Седан"),
            new Ship(0, 0, 500000m, 40, 2018, 250, "Казань")
        };
        foreach (Vehicle vehicle in vehicles)
        { vehicle.Move(2); Console.WriteLine(vehicle.Info()); }
        var sameCar = new Car(180, 0, 150000m, 90, 2022, "Седан");
        Console.WriteLine("Сравнение автомобилей по значениям: " + vehicles[1].Equals(sameCar));
    }
}
