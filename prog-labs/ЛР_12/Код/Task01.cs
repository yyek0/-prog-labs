using System;
using System.Collections.Generic;
using System.Text;

class MyObj_1<T>
{
    private readonly T value;
    public MyObj_1(T value) { this.value = value; }

    public void ShowPower(int power)
    {
        if (typeof(T) == typeof(string))
        {
            Console.WriteLine("Здравствуйте, " + value + "!");
        }
        else if (typeof(T) == typeof(int) || typeof(T) == typeof(double) || typeof(T) == typeof(float))
        {
            Console.WriteLine("{0} в степени {1} = {2}", value, power, Math.Pow(Convert.ToDouble(value), power));
        }
        else throw new NotSupportedException("Допустимы int, double, float и string.");
    }
}

class MyObj<T, Y, U, O>
{
    public T LastName;
    public Y FirstName;
    public U MiddleName;
    public O Detail;
    public MyObj(T last, Y first, U middle, O detail)
    {
        LastName = last; FirstName = first; MiddleName = middle; Detail = detail;
    }
    public override string ToString()
    {
        return string.Format("{0} {1} {2}: {3}", LastName, FirstName, MiddleName, Detail);
    }
}

interface IAccount
{
    string Login { get; set; }
    string Password { get; set; }
}

class AccountMail : IAccount
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Mail { get; set; }
    public DateTime Registered { get; set; }
    public AccountMail() { }
    public AccountMail(string login, string password, string mail, DateTime registered)
    {
        Login = login; Password = password; Mail = mail; Registered = registered;
    }
    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}, {3:dd.MM.yyyy}", Login, Password, Mail, Registered);
    }
}

class Aco<T> where T : AccountMail, new()
{
    private readonly List<T> users = new List<T>();
    public void Add(T user) { users.Add(user); }
    public void Print() { foreach (T user in users) Console.WriteLine(user); }
}

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        new MyObj_1<int>(2).ShowPower(5);
        new MyObj_1<double>(1.5).ShowPower(5);
        new MyObj_1<int>(3).ShowPower(3);
        new MyObj_1<float>(2.5f).ShowPower(3);
        new MyObj_1<string>("Роман").ShowPower(0);

        var people = new[] {
            new MyObj<string, string, string, int>("Ломоносов", "Михаил", "Васильевич", 1711),
            new MyObj<string, string, string, int>("Менделеев", "Дмитрий", "Иванович", 1834),
            new MyObj<string, string, string, int>("Циолковский", "Константин", "Эдуардович", 1857)
        };
        foreach (var person in people) Console.WriteLine(person);
        var books = new[] {
            new MyObj<string, string, string, string>("Толстой", "Лев", "Николаевич", "Война и мир"),
            new MyObj<string, string, string, string>("Достоевский", "Фёдор", "Михайлович", "Преступление и наказание"),
            new MyObj<string, string, string, string>("Пушкин", "Александр", "Сергеевич", "Евгений Онегин")
        };
        foreach (var book in books) Console.WriteLine(book);

        var accounts = new Aco<AccountMail>();
        accounts.Add(new AccountMail("roma", "pass1", "roma@example.com", new DateTime(2025, 1, 10)));
        accounts.Add(new AccountMail("anna", "pass2", "anna@example.com", new DateTime(2025, 2, 12)));
        accounts.Add(new AccountMail("ivan", "pass3", "ivan@example.com", new DateTime(2025, 3, 15)));
        accounts.Print();
    }
}
