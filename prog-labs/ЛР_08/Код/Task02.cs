using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

class Task2
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static double Excellent(int n)
    {
        if(n <= 0)throw new ArgumentException("Число учащихся должно быть положительным.");
        return n * 0.15;
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 8 — результаты контрольной");
        var n = new NumericUpDown
        {
            Location = new Point(200, 20), Minimum = 1, Maximum = 100000, Value = 100
        };
        f.Controls.Add(n);
        Ui.Label(f, "Всего учащихся", 20, 20, 170);
        var result = Ui.Label(f, "", 20, 510);
        var chart = new Chart
        {
            Location = new Point(20, 80), Size = new Size(830, 415)
        };
        chart.ChartAreas.Add("area");
        chart.Legends.Add("legend");
        var s = new Series("Оценки")
        {
            ChartType = SeriesChartType.Pie
        };
        chart.Series.Add(s);
        string[] names =
        {
            "5", "4", "3", "2"
        };
        int[] percent =
        {
            15, 68, 11, 6
        };
        for(int i = 0; i < 4; i++)
        {
            int index = s.Points.AddXY(names[i], percent[i]);
            s.Points[index].LegendText = "Оценка " + names[i];
            s.Points[index].Label = percent[i] + " % ";
        }
        f.Controls.Add(chart);
        Action update = delegate
        {
            double count = Excellent((int)n.Value);
            result.Text = "Оценку 5 получили: " + count.ToString("0.##") + " (15 % от " + n.Value + ").";
            if(count!= Math.Truncate(count))result.Text += " При таком n точные 15 % дают дробное число.";
        };
        Ui.Button(f, "Рассчитать", 370, 15, delegate
        {
            update();
        }
        );
        update();
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


