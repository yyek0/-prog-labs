using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

class Task1
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static int Shortest(string text)
    {
        var words = Regex.Matches(text, @"\p{L} + (?: - \p{L} +) * ");
        if(words.Count == 0)return 0;
        int n = int.MaxValue;
        foreach(Match w in words)n = Math.Min(n, w.Value.Length);
        return n;
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 5 — короткое слово");
        var list = new ListBox
        {
            Location = new Point(25, 65), Size = new Size(820, 260)
        };
        list.Items.AddRange(new object[]
        {
            "Я изучаю язык программирования.", "Сегодня хорошая погода.", "Работа с массивами и строками.", "123 ..."
        }
        );
        f.Controls.Add(list);
        Ui.Label(f, "Выберите строку", 25, 15);
        var answer = Ui.Label(f, "", 25, 350);
        list.SelectedIndexChanged += delegate
        {
            int n = Shortest(Convert.ToString(list.SelectedItem));
            answer.Text = n == 0?"В строке нет слов.":"Длина самого короткого слова: " + n;
        };
        list.SelectedIndex = 0;
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


