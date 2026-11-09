using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

class Task2
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static string[] RemoveExclamations(string text)
    {
        var result = new List < string > ();
        foreach(Match m in Regex.Matches(text, @"[^.!?] + [.!?] * "))
        {
            string s = m.Value.Trim();
            if(s.Length > 0&&!s.Contains("!"))result.Add(s);
        }
        return result.ToArray();
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 5 — предложения");
        var input = new ListBox
        {
            Location = new Point(20, 65), Size = new Size(405, 310)
        };
        var output = new ListBox
        {
            Location = new Point(445, 65), Size = new Size(410, 310)
        };
        input.Items.AddRange(new object[]
        {
            "Привет! Сегодня ясный день. Как дела?", "Отлично! Мы изучаем C#.", "Еще одно предложение! Последняя строка."
        }
        );
        f.Controls.Add(input);
        f.Controls.Add(output);
        Ui.Label(f, "Исходный текст", 20, 15, 400);
        Ui.Label(f, "Без восклицательных предложений", 445, 15, 410);
        Action process = delegate
        {
            output.Items.Clear();
            foreach(string s in RemoveExclamations(string.Join(" ", input.Items.Cast < string > ())))output.Items.Add(s);
        };
        Ui.Button(f, "Обработать", 20, 405, delegate
        {
            process();
        }
        );
        process();
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


