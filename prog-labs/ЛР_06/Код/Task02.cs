using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

class Task2
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static double[] Transform(double[] a)
    {
        if(a.Length!= 9)throw new ArgumentException("Нужно девять элементов.");
        var b = (double[])a.Clone();
        if(b[1] > b[7])
        {
            b[1] /= 2;
            b[7] /= 2;
        }
        else
        {
            b[1] = 0;
            b[7] = 0;
        }
        return b;
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 6 — второй и восьмой элементы");
        var a = new ComboBox
        {
            Location = new Point(25, 90), Size = new Size(390, 35), DropDownStyle = ComboBoxStyle.DropDownList
        };
        var b = new ComboBox
        {
            Location = new Point(445, 90), Size = new Size(390, 35), DropDownStyle = ComboBoxStyle.DropDownList
        };
        f.Controls.Add(a);
        f.Controls.Add(b);
        Ui.Label(f, "Исходный массив", 25, 35, 390);
        Ui.Label(f, "Измененный массив", 445, 35, 390);
        var text = Ui.Label(f, "", 25, 200);
        text.Height = 150;
        double[] values = null;
        var random = new Random();
        Action fill = delegate
        {
            values = Enumerable.Range(0, 9).Select(x => Math.Round(- 4 + 8 * random.NextDouble(), 2)).ToArray();
            a.Items.Clear();
            b.Items.Clear();
            for(int i = 0; i < 9; i++)a.Items.Add((i + 1) + ": " + values[i]);
            a.SelectedIndex = 0;
            text.Text = "Новый массив заполнен.";
        };
        Action process = delegate
        {
            if(values == null)return;
            var result = Transform(values);
            b.Items.Clear();
            for(int i = 0; i < 9; i++)b.Items.Add((i + 1) + ": " + result[i]);
            b.SelectedIndex = 1;
            text.Text = string.Format("До: a[2] = {0}, a[8] = {1}\nПосле: a[2] = {2}, a[8] = {3}", values[1], values[7], result[1], result[7]);
        };
        Ui.Button(f, "Заполнить", 25, 140, delegate
        {
            fill();
        }
        );
        Ui.Button(f, "Обработать", 445, 140, delegate
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


