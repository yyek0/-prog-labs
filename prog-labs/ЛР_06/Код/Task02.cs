using System;
using System.Text;

class Base
{
    public virtual double method(double x, double y) { return x + y; }
}
class Proizv1 : Base
{
    public override double method(double x, double y) { return x * y + 5; }
}
class Proizv2 : Base
{
    public override double method(double x, double y)
    {
        if (y == 0) throw new DivideByZeroException();
        return Math.Tan(x / y);
    }
}
class Proizv3 : Base
{
    public override double method(double x, double y) { return x * x * x - y * y * y; }
}
class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("x: "); double x = double.Parse(Console.ReadLine());
        Console.Write("y: "); double y = double.Parse(Console.ReadLine());
        Base[] calculations = { new Base(), new Proizv1(), new Proizv2(), new Proizv3() };
        foreach (Base calculation in calculations)
            Console.WriteLine("{0}: {1}", calculation.GetType().Name, calculation.method(x, y));
    }
}
