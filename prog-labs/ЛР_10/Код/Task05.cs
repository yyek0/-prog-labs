using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

class Task5
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(Build());

    }
    public static void Tank(Graphics g, float x, float y, float scale)
    {
        var state = g.Save();
        g.TranslateTransform(x, y);
        g.ScaleTransform(scale, scale);
        g.FillEllipse(Brushes.DarkSlateGray, 0, 45, 180, 45);
        g.FillRectangle(Brushes.OliveDrab, 10, 25, 155, 40);
        g.FillRectangle(Brushes.DarkOliveGreen, 55, 0, 70, 30);
        g.FillRectangle(Brushes.DarkOliveGreen, 115, 10, 85, 9);
        for(int i = 0; i < 6; i++)
        {
            g.FillEllipse(Brushes.Gray, 15 + i * 25, 55, 22, 22);
            g.DrawEllipse(Pens.Black, 15 + i * 25, 55, 22, 22);
        }
        g.Restore(state);
    }
    public static float Advance(float x, float width)
    {
        return x > width? - 200:x + 4;
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 10 — метод узловой точки");
        float x = 0;
        bool moving = false;
        var timer = new Timer
        {
            Interval = 40
        };
        timer.Tick += delegate
        {
            x = Advance(x, f.ClientSize.Width);
            f.Invalidate();
        };
        Ui.Button(f, "Старт / стоп", 20, 510, delegate
        {
            moving = !moving; timer.Enabled = moving;
        }
        );
        f.Paint += delegate(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Tank(g, 35, 50, .7f);
            Tank(g, 275, 75, 1);
            Tank(g, 535, 55, 1.4f);
            Tank(g, x, 355, 1);
        };
        f.FormClosed += delegate
        {
            timer.Dispose();
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


