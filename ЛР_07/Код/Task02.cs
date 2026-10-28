using System;
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
    public static double[,] Transform(int[,] a, out double mean)
    {
        long sum = 0;
        foreach(int x in a)sum += x;
        mean = (double)sum / a.Length;
        var b = new double[a.GetLength(0), a.GetLength(1)];
        for(int i = 0; i < a.GetLength(0); i++)for(int j = 0; j < a.GetLength(1); j++)b[i, j] = i == j?mean:a[i, j];
        return b;
    }
    public static Form Build()
    {
        var f = Ui.Form("ЛР 7 — среднее матрицы 15 × 15");
        f.ClientSize = new Size(1080, 760);
        Ui.Label(f, "Верхняя таблица — исходная, нижняя — результат", 20, 10, 1040);
        var input = MatrixInput.Grid(f, 15, 15);
        input.Size = new Size(1035, 290);
        input.Location = new Point(20, 50);
        foreach(DataGridViewColumn c in input.Columns)c.Width = 63;
        var output = new DataGridView
        {
            Location = new Point(20, 395), Size = new Size(1035, 320), AllowUserToAddRows = false, ReadOnly = true, RowHeadersWidth = 60
        };
        for(int j = 0; j < 15; j++)
        {
            output.Columns.Add("c" + j, (j + 1).ToString());
            output.Columns[j].Width = 63;
        }
        output.RowCount = 15;
        f.Controls.Add(output);
        var answer = Ui.Label(f, "", 220, 350, 820);
        Action process = delegate
        {
            input.EndEdit();
            double mean;
            var b = Transform(MatrixInput.Read(input), out mean);
            for(int i = 0; i < 15; i++)
            {
                output.Rows[i].HeaderCell.Value = (i + 1).ToString();
                for(int j = 0; j < 15; j++)
                {
                    output[j, i].Value = b[i, j].ToString("0.###");
                    if(i == j)output[j, i].Style.BackColor = Color.LightYellow;
                }
            }
            answer.Text = "Среднее арифметическое: " + mean.ToString("0.######");
        };
        Ui.Button(f, "Обработать", 20, 345, delegate
        {
            Ui.Guard(process);
        }
        );
        process();
        return f;
    }

}
static class MatrixInput
{
    public static DataGridView Grid(Form form, int rows, int columns)
    {
        var grid = new DataGridView
        {
            Location = new Point(20, 65), Size = new Size(835, 360), AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersWidth = 60
        };
        for (int column = 0; column < columns; column++)
        {
            grid.Columns.Add("c" + column, (column + 1).ToString());
        }
        grid.RowCount = rows;
        var random = new Random(7);
        for (int row = 0; row < rows; row++)
        {
            grid.Rows[row].HeaderCell.Value = (row + 1).ToString();
            for (int column = 0; column < columns; column++)
            {
                grid[column, row].Value = random.Next(- 20, 21);
            }
        }
        form.Controls.Add(grid);
        return grid;
    }
    public static int[,] Read(DataGridView grid)
    {
        var values = new int[grid.RowCount, grid.ColumnCount];
        for (int row = 0; row < grid.RowCount; row++)
        {
            for (int column = 0; column < grid.ColumnCount; column++)
            {
                int value;
                if (!int.TryParse(Convert.ToString(grid[column, row].Value), out value))
                {
                    throw new FormatException("Все ячейки должны содержать целые числа.");
                }
                values[row, column] = value;
            }
        }
        return values;
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


