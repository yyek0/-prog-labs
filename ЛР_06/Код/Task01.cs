using System;
using System.Linq;
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
    public static double[] Transform(double[] d)
    {
        return d.Select(x => 0.02 * x * x - 0.2 * x + 5).ToArray();
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 6 — e = 0, 02d² − 0, 2d + 5");
        var a = new ListBox
        {
            Location = new Point(25, 65), Size = new Size(390, 400)
        };
        var b = new ListBox
        {
            Location = new Point(445, 65), Size = new Size(390, 400)
        };
        f.Controls.Add(a);
        f.Controls.Add(b);
        Ui.Label(f, "Массив d", 25, 15, 350);
        Ui.Label(f, "Массив e", 445, 15, 350);
        double[] values = null;
        var random = new Random();
        Action fill = delegate
        {
            values = Enumerable.Range(0, 12).Select(x => (double)random.Next(- 10, 11)).ToArray();
            a.Items.Clear();
            b.Items.Clear();
            foreach(double x in values)a.Items.Add(x);
        };
        Action process = delegate
        {
            if(values == null)
            {
                MessageBox.Show("Сначала заполните массив.");
                return;
            }
            b.Items.Clear();
            foreach(double x in Transform(values))b.Items.Add(x.ToString("F2"));
        };
        Ui.Button(f, "Заполнить", 25, 490, delegate
        {
            fill();
        }
        );
        Ui.Button(f, "Обработать", 445, 490, delegate
        {
            process();
        }
        );
        fill();
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


