using System;
using System.Text;

class CircleWrite
{
    public double x, y, radius;
    public double X
    {
        private get
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
        private get
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
        private get
        {
            return radius;
        }
        set
        {
            if(value <= 0||double.IsNaN(value)||double.IsInfinity(value))throw new ArgumentException("Недопустимый радиус.");
            radius = value;
        }
    }
    public string Describe()
    {
        return string.Format("Центр ({0}; {1}), радиус {2}", X, Y, Radius);
    }
}
class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        for(int i = 0; i < 5; i++)
        {
            var c = new CircleWrite
            {
                X = i, Y = - i, Radius = i + 1
            };
            Console.WriteLine(c.Describe());
        }

    }

}


