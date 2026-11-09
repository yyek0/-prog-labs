using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

class Task4
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static int Draw(Graphics g, int count, int index = 0)
    {
        if(count < 1||count > 60)throw new ArgumentException("Количество должно быть от 1 до 60.");
        if(index == count)return 0;
        double angle = 2 * Math.PI * index / count;
        float x = 425 + 160 * (float)Math.Cos(angle), y = 255 + 160 * (float)Math.Sin(angle), radius = 8 + index * 3;
        using(var pen = new Pen(Color.FromArgb(30, 80 + index * 2, 150), 2))g.DrawEllipse(pen, x - radius, y - radius, 2 * radius, 2 * radius);
        return 1 + Draw(g, count, index + 1);
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 10 — возрастающие окружности");
        f.Paint += delegate(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Draw(e.Graphics, 12);
        };
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


