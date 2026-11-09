using System;
using System.Text;

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            Rethrow();
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Внешний обработчик получил повторное исключение.");
        }
        try
        {
            throw new InvalidOperationException("Операция запрещена в текущем состоянии.");
        }
        catch(IndexOutOfRangeException)
        {
            Console.WriteLine("Ошибка индекса.");
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Деление на ноль.");
        }
        catch(OverflowException)
        {
            Console.WriteLine("Переполнение.");
        }
        catch(Exception ex)
        {
            Console.WriteLine("Универсальный обработчик: " + ex.GetType().Name + " — " + ex.Message);
        }

    }
    public static void Rethrow()
    {
        try
        {
            int zero = 0;
            Console.WriteLine(1 / zero);
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Внутренний обработчик: деление на ноль.");
            throw;
        }
    }

}


