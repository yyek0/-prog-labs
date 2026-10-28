using System;
using System.Text;

class CircleMethods
{
    protected double x, y, r;
    public void Set(double x, double y, double radius)
    {
        if(double.IsNaN(radius)||double.IsInfinity(radius)||radius <= 0)throw new ArgumentException("Радиус должен быть положительным.");
        this.x = x;
        this.y = y;
        r = radius;
    }
    public double Area()
    {
        return Math.PI * r * r;
    }
    public double Length()
    {
        return 2 * Math.PI * r;
    }
    public bool Contains(double px, double py)
    {
        return (px - x) * (px - x) + (py - y) * (py - y) <= r * r;
    }
    public void Print()
    {
        Console.WriteLine("Центр ({0}; {1}), r = {2}; S = {3:F4}; L = {4:F4}", x, y, r, Area(), Length());
    }
}
class CircleConstructor:CircleMethods
{
    public CircleConstructor(double x, double y, double r)
    {
        Set(x, y, r);
    }
}
class CircleOverload:CircleMethods
{
    public CircleOverload()
    {
        Set(0, 0, 1);
    }
    public CircleOverload(double r)
    {
        Set(0, 0, r);
    }
    public CircleOverload(double x, double y, double r)
    {
        Set(x, y, r);
    }
}
class CircleChain:CircleMethods
{
    public CircleChain():this(1)
    {
    }
    public CircleChain(double r):this(0, 0, r)
    {
    }
    public CircleChain(double x, double y, double r)
    {
        Set(x, y, r);
    }
}
class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var a = new CircleMethods();
        a.Set(0, 0, 2);
        a.Print();
        new CircleConstructor(1, 2, 3).Print();
        new CircleOverload().Print();
        new CircleOverload(2).Print();
        new CircleOverload(1, 2, 3).Print();
        new CircleChain().Print();
        new CircleChain(2).Print();
        var b = new CircleChain(1, 2, 3);
        b.Print();
        Console.WriteLine("Точка (2;2) внутри круга: " + b.Contains(2, 2));
        Console.WriteLine("Точка (6;2) внутри круга: " + b.Contains(6, 2));

    }

}


