using System;
using System.Text;

interface INomer
{
    string Otmetka(byte a);
}

enum ExamMark : byte
{
    One = 1, Two = 2, Three = 3, Four = 4, Five = 5
}

class MarkDetails
{
    public string Comment;
    public MarkDetails(string comment) { Comment = comment; }
    public MarkDetails Copy() { return new MarkDetails(Comment); }
}

class Nazvanie : INomer, ICloneable
{
    public MarkDetails Details { get; private set; }
    public Nazvanie(string comment) { Details = new MarkDetails(comment); }

    public string Otmetka(byte a)
    {
        if (!Enum.IsDefined(typeof(ExamMark), a))
            throw new ArgumentOutOfRangeException("a", "Оценка должна быть от 1 до 5.");

        switch ((ExamMark)a)
        {
            case ExamMark.One: return "очень плохо";
            case ExamMark.Two: return "неудовлетворительно";
            case ExamMark.Three: return "удовлетворительно";
            case ExamMark.Four: return "хорошо";
            default: return "отлично";
        }
    }

    // Обычный MemberwiseClone копирует ссылку на Details.
    public object Clone() { return MemberwiseClone(); }

    public Nazvanie DeepCopy()
    {
        var copy = (Nazvanie)MemberwiseClone();
        copy.Details = Details.Copy();
        return copy;
    }
}

class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var mark = new Nazvanie("первая попытка");
        Console.WriteLine("Через объект: " + mark.Otmetka(5));

        INomer throughInterface = mark;
        Console.WriteLine("Через интерфейс: " + throughInterface.Otmetka(4));
        Console.WriteLine("Проверка is: " + (mark is INomer));
        INomer throughAs = mark as INomer;
        Console.WriteLine("Проверка as: " + (throughAs != null));

        var shallow = (Nazvanie)mark.Clone();
        var deep = mark.DeepCopy();
        mark.Details.Comment = "исправленная работа";
        Console.WriteLine("Оригинал: " + mark.Details.Comment);
        Console.WriteLine("Поверхностная копия: " + shallow.Details.Comment);
        Console.WriteLine("Глубокая копия: " + deep.Details.Comment);
    }
}
