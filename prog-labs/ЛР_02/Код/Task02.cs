using System;
using System.Linq;
using System.Text;

class Chislo
{
    readonly string name;
    int number;
    public string Name
    {
        get
        {
            return name;
        }
    }
    public Chislo(string name)
    {
        if(string.IsNullOrWhiteSpace(name))throw new ArgumentException("Имя не должно быть пустым.");
        this.name = name;
    }
    public void Think(Random random, int k, int m)
    {
        number = random.Next(k, m + 1);
    }
    public bool Guess(int guess)
    {
        return guess == number;
    }
}
class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string[] names =
        {
            "Анна", "Борис", "Вера", "Глеб", "Дина", "Егор", "Жанна"
        };
        var players = names.Select(x => new Chislo(x)).ToArray();
        var random = new Random();
        foreach(var p in players)p.Think(random, 2, 18);
        Console.WriteLine("7 участников загадали числа от 2 до 18. У вас 4 попытки.");
        for(int attempt = 1; attempt <= 4;)
        {
            Console.WriteLine("Попытка " + attempt + ":");
            int n;
            if(!int.TryParse(Console.ReadLine(), out n)||n < 2||n > 18)
            {
                Console.WriteLine("Введите целое число от 2 до 18.");
                if(Console.IsInputRedirected)return;
                continue;
            }
            var found = players.Where(x => x.Guess(n)).Select(x => x.Name).ToArray();
            Console.WriteLine(found.Length == 0?"Совпадений нет.":"Угаданы числа участников: " + string.Join(", ", found));
            attempt++ ;
        }
        Console.WriteLine("Попытки закончились.");

    }

}


