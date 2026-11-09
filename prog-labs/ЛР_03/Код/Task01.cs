using System;
using System.Text;

class Circle
{
    private double x, y, radius;
    public double X
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }
    public double Y
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }
    public double Radius
    {
        get
        {
            return radius;
        }
        set
        {
            if(double.IsNaN(value)||double.IsInfinity(value)||value <= 0)throw new ArgumentException("Радиус должен быть положительным.");
            radius = value;
        }
    }
    public override string ToString()
    {
        return string.Format("Центр ({0}; {1}), радиус {2}", X, Y, Radius);
    }
}
class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        for(int i = 0; i < 5; i++)
        {
            var c = new Circle
            {
                X = i, Y = i + 1, Radius = i + 2
            };
            Console.WriteLine(c);
        }

    }

}


