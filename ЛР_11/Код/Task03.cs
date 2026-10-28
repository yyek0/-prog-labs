using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class Student
{
    public string Last, First, Middle, Place;
    public int Year;
    public override string ToString()
    {
        return Last + " " + First + " " + Middle + "; " + Year + "; " + Place;
    }
}
class Task3
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static Student[] Find(string path, string last)
    {
        var result = new List < Student > ();
        foreach(string line in File.ReadAllLines(path, Encoding.UTF8))
        {
            if(string.IsNullOrWhiteSpace(line))continue;
            var a = line.Split(';');
            int year;
            if(a.Length!= 5||!int.TryParse(a[3], out year))throw new FormatException("Неверная строка анкеты: " + line);
            if(string.Equals(a[0], last.Trim(), StringComparison.OrdinalIgnoreCase))result.Add(new Student
            {
                Last = a[0], First = a[1], Middle = a[2], Year = year, Place = a[4]
            }
            );
        }
        return result.ToArray();
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 11 — анкета");
        string path = Path.Combine(Environment.CurrentDirectory, "students.txt");
        if(!File.Exists(path))File.WriteAllLines(path, new[]
        {
            "Иванов;Иван;Иванович;2007;Бирск", "Петрова;Анна;Сергеевна;2006;Уфа", "Иванов;Петр;Алексеевич;2005;Казань"
        }
        , Encoding.UTF8);
        Ui.Label(f, "Фамилия студента (демонстрационные записи)", 20, 20);
        var name = new TextBox
        {
            Location = new Point(20, 75), Width = 420, Text = "Иванов"
        };
        f.Controls.Add(name);
        var output = new TextBox
        {
            Location = new Point(20, 180), Size = new Size(835, 300), Multiline = true, ReadOnly = true
        };
        f.Controls.Add(output);
        Action search = delegate
        {
            var found = Find(path, name.Text);
            output.Text = found.Length == 0?"Студент не найден.":string.Join("\r\n", found.Select(x => x.ToString()));
        };
        Ui.Button(f, "Найти", 470, 70, delegate
        {
            Ui.Guard(search);
        }
        );
        search();
        return f;
    }

}
static class Ui
{
    public static Form Form(string title)
    {
        return new Form
        {
            Text = title, ClientSize = new Size(880, 580), Font = new Font("Segoe UI", 11), StartPosition = FormStartPosition.CenterScreen
        };
    }
    public static Button Button(Control parent, string text, int x, int y, EventHandler click)
    {
        var button = new Button
        {
            Text = text, Location = new Point(x, y), Size = new Size(180, 36)
        };
        button.Click += click;
        parent.Controls.Add(button);
        return button;
    }
    public static Label Label(Control parent, string text, int x, int y, int width = 820)
    {
        var label = new Label
        {
            Text = text, Location = new Point(x, y), Size = new Size(width, 52)
        };
        parent.Controls.Add(label);
        return label;
    }
    public static void Guard(Action action)
    {
        action();
    }
}


