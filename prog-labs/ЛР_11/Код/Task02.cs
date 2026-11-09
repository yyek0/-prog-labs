using System;
using System.IO;
using System.Linq;
using System.Text;
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
    public static int[] Generate(string path, int n)
    {
        if(n < 1||n > 1000)throw new ArgumentException("Количество должно быть от 1 до 1000.");
        var random = new Random();
        int[] a = Enumerable.Range(0, n).Select(i => random.Next(- 19, 7)).ToArray();
        File.WriteAllLines(path, a.Select(x => x.ToString()).ToArray(), Encoding.UTF8);
        return a;
    }
    public static long Product(int[] a, int divisor, out int count)
    {
        if(divisor == 0)throw new ArgumentException("A не должно быть равно нулю.");
        long result = 1;
        count = 0;
        for(int i = 1; i < a.Length; i += 2)if((long)a[i] % divisor == 0)
        {
            result = checked(result * a[i]);
            count++ ;
        }
        return result;
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 11 — числа в файле");
        Ui.Label(f, "A (делитель)", 20, 20, 150);
        var number = new NumericUpDown
        {
            Location = new Point(175, 20), Minimum = - 100, Maximum = 100, Value = 3
        };
        f.Controls.Add(number);
        var text = new TextBox
        {
            Location = new Point(20, 90), Size = new Size(835, 310), Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical
        };
        f.Controls.Add(text);
        var result = Ui.Label(f, "", 20, 465);
        Action process = delegate
        {
            string path = Path.Combine(Environment.CurrentDirectory, "numbers.txt");
            Generate(path, 20);
            var a = File.ReadAllLines(path, Encoding.UTF8).Select(int.Parse).ToArray();
            text.Text = string.Join("\r\n", a.Select((x, i) => (i + 1) + ": " + x));
            int count;
            long product = Product(a, (int)number.Value, out count);
            result.Text = count == 0?"Подходящих элементов нет.":"Найдено: " + count + ". Произведение: " + product;
        };
        Ui.Button(f, "Создать и обработать", 20, 415, delegate
        {
            Ui.Guard(process);
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


