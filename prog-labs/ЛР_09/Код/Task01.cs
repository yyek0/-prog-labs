using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
    public static void Draw(Graphics g, int width, int height)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var state = g.Save();
        g.ScaleTransform(width / 800f, height / 500f);
        g.Clear(Color.Cyan);
        g.FillRectangle(Brushes.Navy, 0, 300, 800, 200);
        g.FillEllipse(Brushes.Yellow, 650, 40, 65, 65);
        g.FillEllipse(Brushes.White, 40, 45, 180, 14);
        g.FillEllipse(Brushes.White, 210, 65, 240, 18);
        g.FillPolygon(Brushes.SaddleBrown, new[]
        {
            new Point(275, 265), new Point(535, 265), new Point(485, 300), new Point(315, 300)
        }
        );
        g.DrawLine(Pens.Black, 405, 150, 405, 265);
        g.FillPolygon(Brushes.Red, new[]
        {
            new Point(402, 150), new Point(310, 260), new Point(402, 260)
        }
        );
        g.FillPolygon(Brushes.LightGray, new[]
        {
            new Point(460, 380), new Point(510, 355), new Point(675, 380), new Point(510, 400)
        }
        );
        g.FillPolygon(Brushes.LightGray, new[]
        {
            new Point(670, 380), new Point(715, 355), new Point(715, 405)
        }
        );
        g.FillEllipse(Brushes.Red, 476, 375, 9, 9);
        g.Restore(state);
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 9 — компьютерная графика, вариант 7");
        f.ClientSize = new Size(800, 500);
        f.Paint += delegate(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics, f.ClientSize.Width, f.ClientSize.Height);
        };
        f.Resize += delegate
        {
            f.Invalidate();
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


