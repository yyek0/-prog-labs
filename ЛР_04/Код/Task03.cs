using System;
using System.Text;

class Task3
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        byte i = 255;
        Console.WriteLine("Без проверки: " + unchecked((byte)(i + 1)));
        try
        {
            checked
            {
                i++ ;
            }
        }
        catch(OverflowException)
        {
            Console.WriteLine("С checked получено OverflowException.");
        }
        Console.WriteLine("Для цикла до 500 нужна переменная int. Последнее значение: 499.");

    }

}


