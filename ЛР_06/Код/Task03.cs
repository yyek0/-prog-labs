using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

class Task3
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static int Count(int[] a)
    {
        return a.Count(x => x > 0&&x % 2!= 0);
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 6 — подсчет элементов");
        var box = new TextBox
        {
            Location = new Point(25, 80), Size = new Size(820, 90), Multiline = true
        };
        f.Controls.Add(box);
        Ui.Label(f, "Введите 25 целых чисел через пробел", 25, 20);
        box.Text = string.Join(" ", Enumerable.Range(- 12, 25));
        var answer = Ui.Label(f, "", 25, 240);
        Ui.Button(f, "Посчитать", 25, 185, delegate
        {
            Ui.Guard(delegate
            {
                var a = box.Text.Split(new[]
                {
                    ' ', '\r', '\n', ';'
                }
                , StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(); if(a.Length!= 25)throw new ArgumentException("Нужно ровно 25 чисел."); answer.Text = "Нечетных положительных элементов: " + Count(a);
            }
            );
        }
        );
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


