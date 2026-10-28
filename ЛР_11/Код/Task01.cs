using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

class Task1
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static string Info(string path)
    {
        var f = new FileInfo(path);
        if(!f.Exists)throw new FileNotFoundException("Файл не найден.", path);
        return string.Format("Имя: {0}\r\nПолный путь: {1}\r\nРазмер: {2} байт\r\nСоздан: {3}\r\nИзменен: {4}\r\nРасширение: {5}\r\nАтрибуты: {6}", f.Name, f.FullName, f.Length, f.CreationTime, f.LastWriteTime, f.Extension, f.Attributes);
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 11 — информация о файле");
        var path = new TextBox
        {
            Location = new Point(20, 25), Width = 620
        };
        var result = new TextBox
        {
            Location = new Point(20, 100), Size = new Size(835, 400), Multiline = true, ReadOnly = true
        };
        f.Controls.Add(path);
        f.Controls.Add(result);
        Ui.Button(f, "Выбрать файл", 670, 20, delegate
        {
            using(var d = new OpenFileDialog())
            {
                if(d.ShowDialog() == DialogResult.OK)
                {
                    path.Text = d.FileName; Ui.Guard(delegate
                    {
                        result.Text = Info(path.Text);
                    }
                    );
                }
            }
        }
        );
        Ui.Button(f, "Показать", 20, 525, delegate
        {
            Ui.Guard(delegate
            {
                result.Text = Info(path.Text);
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


