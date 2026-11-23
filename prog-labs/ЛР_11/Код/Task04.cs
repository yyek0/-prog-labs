using System;
using System.Text;

class Counter
{
    public event Action<int> Reached;
    public void Count(int last, Func<int, bool> condition)
    {
        for (int i = 1; i <= last; i++)
            if (condition(i) && Reached != null) Reached(i);
    }
}

class Wallet
{
    public int Balance { get; private set; }
    public event Action<int, int> Paid;
    public event Action<int> Added;
    public Wallet(int balance) { Balance = balance; }
    public bool Pay(int amount)
    {
        if (amount <= 0 || amount > Balance) return false;
        Balance -= amount;
        if (Paid != null) Paid(amount, Balance);
        return true;
    }
    public void Add(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException("amount");
        Balance += amount;
        if (Added != null) Added(Balance);
    }
}

class OwnerCheck
{
    public event Action<string> Matched;
    public void Check(string owner, string entered)
    {
        if (string.Equals(owner, entered, StringComparison.OrdinalIgnoreCase) && Matched != null)
            Matched(owner);
    }
}

class Task4
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var counter = new Counter();
        counter.Reached += n => Console.WriteLine("Первое сообщение: " + n);
        counter.Reached += n => Console.WriteLine("Второе сообщение: " + n);
        counter.Count(100, n => n == 50 || n == 90);
        counter.Count(90, n => n % 10 == 0);

        var wallet = new Wallet(100);
        wallet.Paid += (amount, balance) => Console.WriteLine("Платёж " + amount);
        wallet.Paid += (amount, balance) => Console.WriteLine("Списано " + amount);
        wallet.Paid += (amount, balance) => Console.WriteLine("Остаток " + balance);
        wallet.Added += balance => Console.WriteLine("После пополнения: " + balance);
        wallet.Pay(40);
        wallet.Add(25);

        var password = new OwnerCheck();
        password.Matched += name => Console.WriteLine("Пароль совпал с владельцем: " + name);
        var auto = new OwnerCheck();
        auto.Matched += name => Console.WriteLine("Номер машины совпал с владельцем: " + name);
        string[] attempts = { "Иван", "роман", "Роман", "12", "Роман" };
        foreach (string attempt in attempts)
        {
            password.Check("Роман", attempt);
            auto.Check("Роман", attempt);
        }
    }
}
