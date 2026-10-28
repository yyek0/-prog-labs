using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

class Task1
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static double F(double x)
    {
        return 0.25 - 0.25 * Math.Sin(2.5 * Math.PI - 8 * x);
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 8 — график функции");
        var chart = new Chart
        {
            Location = new Point(20, 70), Size = new Size(590, 475)
        };
        chart.ChartAreas.Add(new ChartArea("plot"));
        chart.ChartAreas[0].AxisX.Title = "x, рад";
        chart.ChartAreas[0].AxisY.Title = "y";
        chart.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";
        chart.ChartAreas[0].AxisX.Minimum = - Math.PI;
        chart.ChartAreas[0].AxisX.Maximum = Math.PI;
        chart.ChartAreas[0].AxisX.Interval = Math.PI / 2;
        var s = new Series("y")
        {
            ChartType = SeriesChartType.Line, BorderWidth = 2
        };
        chart.Series.Add(s);
        f.Controls.Add(chart);
        var grid = new DataGridView
        {
            Location = new Point(625, 70), Size = new Size(230, 475), ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false
        };
        grid.Columns.Add("x", "x");
        grid.Columns.Add("y", "y");
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        f.Controls.Add(grid);
        Ui.Label(f, "y = 1 / 4 − 1 / 4 · sin(5π / 2 − 8x); x ∈ [−π; π], h = π / 64", 20, 15);
        for(int i = 0; i <= 128; i++)
        {
            double x = - Math.PI + i * Math.PI / 64, y = F(x);
            s.Points.AddXY(x, y);
            grid.Rows.Add(x.ToString("F4"), y.ToString("F4"));
        }
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


