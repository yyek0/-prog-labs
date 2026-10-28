using System;
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
    public static int[] MaxColumns(int[,] a)
    {
        int[] result = new int[a.GetLength(1)];
        for(int j = 0; j < result.Length; j++)
        {
            result[j] = a[0, j];
            for(int i = 1; i < a.GetLength(0); i++)result[j] = Math.Max(result[j], a[i, j]);
        }
        return result;
    }
    public static DataGridView Grid(Form f, int rows, int cols)
    {
        var g = new DataGridView
        {
            Location = new Point(20, 65), Size = new Size(835, 360), AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersWidth = 60
        };
        for(int j = 0; j < cols; j++)g.Columns.Add("c" + j, (j + 1).ToString());
        g.RowCount = rows;
        var r = new Random(7);
        for(int i = 0; i < rows; i++)
        {
            g.Rows[i].HeaderCell.Value = (i + 1).ToString();
            for(int j = 0; j < cols; j++)g[j, i].Value = r.Next(- 20, 21);
        }
        f.Controls.Add(g);
        return g;
    }
    public static int[,] Read(DataGridView g)
    {
        var a = new int[g.RowCount, g.ColumnCount];
        for(int i = 0; i < g.RowCount; i++)for(int j = 0; j < g.ColumnCount; j++)
        {
            int x;
            if(!int.TryParse(Convert.ToString(g[j, i].Value), out x))throw new FormatException("Все ячейки должны содержать целые числа.");
            a[i, j] = x;
        }
        return a;
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 7 — матрица 5 × 3");
        Ui.Label(f, "Можно изменить значения в таблице", 20, 15);
        var g = Grid(f, 5, 3);
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        var answer = Ui.Label(f, "", 220, 455, 620);
        Ui.Button(f, "Максимумы", 20, 455, delegate
        {
            Ui.Guard(delegate
            {
                g.EndEdit(); answer.Text = "Максимумы столбцов: " + string.Join("; ", MaxColumns(Read(g)));
            }
            );
        }
        );
        answer.Text = "Максимумы столбцов: " + string.Join("; ", MaxColumns(Read(g)));
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


