using System;
using System.Text;

interface IPlayable
{
    void Play();
    void Pause();
    void Stop();
}

interface IRecodable
{
    void Record();
    void Pause();
    void Stop();
}

class Player : IPlayable, IRecodable
{
    public void Play() { Console.WriteLine("Воспроизведение началось"); }
    public void Record() { Console.WriteLine("Запись началась"); }

    // Одинаковые имена в интерфейсах различаются при вызове через интерфейс.
    void IPlayable.Pause() { Console.WriteLine("Воспроизведение на паузе"); }
    void IPlayable.Stop() { Console.WriteLine("Воспроизведение остановлено"); }
    void IRecodable.Pause() { Console.WriteLine("Запись на паузе"); }
    void IRecodable.Stop() { Console.WriteLine("Запись остановлена"); }
}

class Task2
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var player = new Player();
        IPlayable play = player;
        IRecodable record = player;
        play.Play();
        play.Pause();
        play.Stop();
        record.Record();
        record.Pause();
        record.Stop();
    }
}
