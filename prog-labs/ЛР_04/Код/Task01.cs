using System;
using System.Text;

class Massiv
{
    readonly Random random = new Random(7);
    public void Formir(ref int[] arr, int a, int b)
    {
        try
        {
            if(arr == null)throw new ArgumentNullException("arr");
            if(a > b)throw new ArgumentException("Левая граница больше правой.");
            int end = checked(b + 1);
            for(int i = 0; i < arr.Length; i++)arr[i] = random.Next(a, end);
        }
        catch(IndexOutOfRangeException)
        {
            Console.WriteLine("Индекс вне массива.");
            throw;
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Деление на ноль.");
            throw;
        }
        catch(OverflowException)
        {
            Console.WriteLine("Переполнение границы.");
            throw;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public void Vivod(ref int[] arr)
    {
        try
        {
            if(arr == null)throw new ArgumentNullException("arr");
            foreach(int n in arr)Console.WriteLine(n);
        }
        catch(IndexOutOfRangeException)
        {
            Console.WriteLine("Индекс вне массива.");
            throw;
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Деление на ноль.");
            throw;
        }
        catch(OverflowException)
        {
            Console.WriteLine("Переполнение.");
            throw;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public void Operasia1(ref int[] arr, out int result)
    {
        result = 0;
        try
        {
            if(arr == null)throw new ArgumentNullException("arr");
            foreach(int n in arr)if(n < 0)result = checked(result + n);
        }
        catch(IndexOutOfRangeException)
        {
            Console.WriteLine("Индекс вне массива.");
            throw;
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Деление на ноль.");
            throw;
        }
        catch(OverflowException)
        {
            Console.WriteLine("Переполнение суммы.");
            throw;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public void Sort_st(ref int[] arr)
    {
        try
        {
            if(arr == null)throw new ArgumentNullException("arr");
            Quick(arr, 0, arr.Length - 1);
        }
        catch(IndexOutOfRangeException)
        {
            Console.WriteLine("Индекс вне массива.");
            throw;
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Деление на ноль.");
            throw;
        }
        catch(OverflowException)
        {
            Console.WriteLine("Переполнение.");
            throw;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    void Quick(int[] a, int left, int right)
    {
        if(left >= right)return;
        int i = left, j = right, pivot = a[left + (right - left) / 2];
        while(i <= j)
        {
            while(a[i] > pivot)i++ ;
            while(a[j] < pivot)j-- ;
            if(i <= j)
            {
                int t = a[i];
                a[i] = a[j];
                a[j] = t;
                i++ ;
                j-- ;
            }
        }
        if(left < j)Quick(a, left, j);
        if(i < right)Quick(a, i, right);
    }
}
class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            var m = new Massiv();
            int[] a = new int[9];
            m.Formir(ref a, - 19, 19);
            Console.WriteLine("Исходный массив:");
            m.Vivod(ref a);
            int sum;
            m.Operasia1(ref a, out sum);
            Console.WriteLine("Сумма отрицательных: " + sum);
            m.Sort_st(ref a);
            Console.WriteLine("По убыванию:");
            m.Vivod(ref a);
        }
        catch(IndexOutOfRangeException)
        {
            Console.WriteLine("Ошибка индекса в Main.");
        }
        catch(DivideByZeroException)
        {
            Console.WriteLine("Деление на ноль в Main.");
        }
        catch(OverflowException)
        {
            Console.WriteLine("Переполнение в Main.");
        }
        catch(Exception ex)
        {
            Console.WriteLine("Общая ошибка: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Обработка завершена.");
        }

    }

}


