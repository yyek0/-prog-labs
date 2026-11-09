using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

class Editor:Form
{
    public Bitmap Canvas
    {
        get;
        private set;
    }
    public Color PenColor = Color.Black;
    public float PenWidth = 5;
    public DashStyle PenStyle = DashStyle.Solid;
    PictureBox picture = new PictureBox();
    Panel panel = new Panel();
    Label coordinates = new Label();
    List < Bitmap > history = new List < Bitmap > ();
    int index = - 1;
    bool drawing, eraser, dirty;
    Point old;
    ImageList icons = new ImageList();
    List < ToolStripMenuItem > styleItems = new List < ToolStripMenuItem > ();
    public Editor()
    {
        Text = "ЛР 12 — графический редактор";
        ClientSize = new Size(960, 700);
        Font = new Font("Segoe UI", 10);
        var menu = new MenuStrip();
        var file = new ToolStripMenuItem("Файл");
        var edit = new ToolStripMenuItem("Правка");
        var penMenu = new ToolStripMenuItem("Перо");
        var help = new ToolStripMenuItem("Справка");
        menu.Items.AddRange(new ToolStripItem[]
        {
            file, edit, penMenu, help
        }
        );
        Controls.Add(menu);
        MainMenuStrip = menu;
        Add(file, "Создать", Keys.Control|Keys.N, delegate
        {
            if(ConfirmDiscard())NewImage(850, 500);
        }
        );
        Add(file, "Открыть", Keys.Control|Keys.O, delegate
        {
            OpenDialog();
        }
        );
        Add(file, "Сохранить", Keys.Control|Keys.S, delegate
        {
            SaveDialog();
        }
        );
        Add(file, "Выход", Keys.Alt|Keys.F4, delegate
        {
            Close();
        }
        );
        Add(edit, "Отменить", Keys.Control|Keys.Z, delegate
        {
            Undo();
        }
        );
        Add(edit, "Повторить", Keys.Control|Keys.Y, delegate
        {
            Redo();
        }
        );
        Add(penMenu, "Цвет", Keys.Control|Keys.K, delegate
        {
            ChooseColor();
        }
        );
        var styles = new ToolStripMenuItem("Стиль");
        penMenu.DropDownItems.Add(styles);
        string[] names =
        {
            "Сплошная", "Точки", "Штрих и две точки"
        };
        DashStyle[] values =
        {
            DashStyle.Solid, DashStyle.Dot, DashStyle.DashDotDot
        };
        var combo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList, Width = 170
        };
        combo.Items.AddRange(names);
        for(int i = 0; i < 3; i++)
        {
            int current = i;
            var item = new ToolStripMenuItem(names[i]);
            item.Click += delegate
            {
                combo.SelectedIndex = current;
            };
            styles.DropDownItems.Add(item);
            styleItems.Add(item);
        }
        combo.SelectedIndexChanged += delegate
        {
            PenStyle = values[combo.SelectedIndex];
            for(int i = 0; i < styleItems.Count; i++)styleItems[i].Checked = i == combo.SelectedIndex;
        };
        combo.SelectedIndex = 0;
        Add(help, "О программе", Keys.F1, delegate
        {
            MessageBox.Show("Графический редактор 1.0\nКашапов Роман Радикович\nЛевая кнопка — перо, правая — белый ластик.\nДоступны открытие, сохранение, отмена и повтор.", "О программе");
        }
        );
        var tools = new ToolStrip
        {
            Dock = DockStyle.Top
        };
        icons.ImageSize = new Size(16, 16);
        Color[] colors =
        {
            Color.SteelBlue, Color.Goldenrod, Color.SeaGreen, Color.MediumPurple
        };
        foreach(var color in colors)
        {
            var b = new Bitmap(16, 16);
            using(var g = Graphics.FromImage(b))
            {
                g.Clear(Color.White);
                using(var brush = new SolidBrush(color))g.FillRectangle(brush, 2, 2, 12, 12);
            }
            icons.Images.Add(b);
        }
        string[] commands =
        {
            "Создать", "Открыть", "Сохранить", "Цвет"
        };
        Action[] actions =
        {
            delegate
            {
                if(ConfirmDiscard())NewImage(850, 500);
            }
            , OpenDialog, delegate
            {
                SaveDialog();
            }
            , ChooseColor
        };
        for(int i = 0; i < 4; i++)
        {
            int k = i;
            var button = new ToolStripButton(commands[i], icons.Images[i]);
            button.Click += delegate
            {
                actions[k]();
            };
            tools.Items.Add(button);
        }
        var undo = new ToolStripButton("Отменить");
        undo.Click += delegate
        {
            Undo();
        };
        tools.Items.Add(undo);
        var redo = new ToolStripButton("Повторить");
        redo.Click += delegate
        {
            Redo();
        };
        tools.Items.Add(redo);
        Controls.Add(tools);
        tools.Location = new Point(0, menu.Height);
        var bottom = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(10)
        };
        bottom.Controls.Add(new Label
        {
            Text = "Толщина", AutoSize = true
        }
        );
        var track = new TrackBar
        {
            Minimum = 1, Maximum = 20, Value = 5, Width = 180, Height = 40
        };
        track.ValueChanged += delegate
        {
            PenWidth = track.Value;
        };
        bottom.Controls.Add(track);
        bottom.Controls.Add(combo);
        coordinates.Size = new Size(210, 30);
        bottom.Controls.Add(coordinates);
        Controls.Add(bottom);
        panel.Dock = DockStyle.Fill;
        panel.AutoScroll = true;
        panel.BackColor = Color.LightGray;
        panel.Padding = new Padding(10);
        Controls.Add(panel);
        panel.BringToFront();
        panel.Controls.Add(picture);
        picture.Location = new Point(10, 10);
        picture.SizeMode = PictureBoxSizeMode.AutoSize;
        picture.MouseDown += delegate(object s, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left||e.Button == MouseButtons.Right)
            {
                BeginStroke(e.Location, e.Button == MouseButtons.Right);
                picture.Capture = true;
            }
        };
        picture.MouseMove += delegate(object s, MouseEventArgs e)
        {
            coordinates.Text = "X: " + e.X + " Y: " + e.Y;
            if(drawing)MoveStroke(e.Location);
        };
        picture.MouseUp += delegate
        {
            EndStroke();
            picture.Capture = false;
        };
        FormClosing += delegate(object s, FormClosingEventArgs e)
        {
            if(!ConfirmDiscard())e.Cancel = true;
        };
        NewImage(850, 500);
    }
    void Add(ToolStripMenuItem parent, string text, Keys shortcut, Action action)
    {
        var item = new ToolStripMenuItem(text);
        item.ShortcutKeys = shortcut;
        item.ShowShortcutKeys = true;
        item.Click += delegate
        {
            Ui.Guard(action);
        };
        parent.DropDownItems.Add(item);
    }
    void Replace(Bitmap next)
    {
        var oldImage = Canvas;
        Canvas = next;
        picture.Image = Canvas;
        if(oldImage!= null)oldImage.Dispose();
    }
    void ClearHistory()
    {
        foreach(var b in history)b.Dispose();
        history.Clear();
        index = - 1;
    }
    void Commit()
    {
        while(history.Count > index + 1)
        {
            history[history.Count - 1].Dispose();
            history.RemoveAt(history.Count - 1);
        }
        history.Add(new Bitmap(Canvas));
        if(history.Count > 11)
        {
            history[0].Dispose();
            history.RemoveAt(0);
        }
        index = history.Count - 1;
        dirty = true;
    }
    public void NewImage(int w, int h)
    {
        if(w <= 0||h <= 0)throw new ArgumentException("Недопустимый размер.");
        var b = new Bitmap(w, h);
        using(var g = Graphics.FromImage(b))g.Clear(Color.White);
        Replace(b);
        ClearHistory();
        Commit();
        dirty = false;
    }
    public void BeginStroke(Point point, bool erase)
    {
        drawing = true;
        eraser = erase;
        old = point;
        MoveStroke(point);
    }
    public void MoveStroke(Point point)
    {
        if(!drawing)return;
        using(var g = Graphics.FromImage(Canvas))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using(var pen = new Pen(eraser?Color.White:PenColor, PenWidth))
            {
                pen.DashStyle = eraser?DashStyle.Solid:PenStyle;
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                if(point == old)
                {
                    using(var b = new SolidBrush(pen.Color))g.FillEllipse(b, point.X - PenWidth / 2, point.Y - PenWidth / 2, PenWidth, PenWidth);
                }
                else g.DrawLine(pen, old, point);
            }
        }
        old = point;
        picture.Invalidate();
    }
    public void EndStroke()
    {
        if(!drawing)return;
        drawing = false;
        Commit();
    }
    public void Undo()
    {
        if(index > 0)
        {
            index-- ;
            Replace(new Bitmap(history[index]));
            dirty = true;
        }
    }
    public void Redo()
    {
        if(index + 1 < history.Count)
        {
            index++ ;
            Replace(new Bitmap(history[index]));
            dirty = true;
        }
    }
    public void SaveTo(string path)
    {
        string ext = Path.GetExtension(path).ToLowerInvariant();
        var format = ext == ".jpg"||ext == ".jpeg"?System.Drawing.Imaging.ImageFormat.Jpeg:ext == ".bmp"?System.Drawing.Imaging.ImageFormat.Bmp:System.Drawing.Imaging.ImageFormat.Png;
        Canvas.Save(path, format);
        dirty = false;
    }
    public void OpenFrom(string path)
    {
        Bitmap b;
        using(var source = Image.FromFile(path))
        {
            b = new Bitmap(source.Width, source.Height);
            using(var g = Graphics.FromImage(b))
            {
                g.Clear(Color.White);
                g.DrawImageUnscaled(source, 0, 0);
            }
        }
        Replace(b);
        ClearHistory();
        Commit();
        dirty = false;
    }
    bool SaveDialog()
    {
        using(var d = new SaveFileDialog
        {
            Filter = "PNG| * .png|JPEG| * .jpg|BMP| * .bmp", DefaultExt = "png", AddExtension = true
        }
        )
        {
            if(d.ShowDialog()!= DialogResult.OK)return false;
            SaveTo(d.FileName);
            return true;
        }
    }
    bool ConfirmDiscard()
    {
        if(!dirty)return true;
        var answer = MessageBox.Show("Сохранить изменения?", "Рисунок изменен", MessageBoxButtons.YesNoCancel);
        if(answer == DialogResult.Cancel)return false;
        if(answer == DialogResult.Yes)return SaveDialog();
        return true;
    }
    void OpenDialog()
    {
        if(!ConfirmDiscard())return;
        using(var d = new OpenFileDialog
        {
            Filter = "Изображения| * .png; * .jpg; * .jpeg; * .bmp; * .gif"
        }
        )if(d.ShowDialog() == DialogResult.OK)Ui.Guard(delegate
        {
            OpenFrom(d.FileName);
        }
        );
    }
    void ChooseColor()
    {
        using(var d = new ColorDialog
        {
            Color = PenColor
        }
        )if(d.ShowDialog() == DialogResult.OK)PenColor = d.Color;
    }
    protected override void Dispose(bool disposing)
    {
        if(disposing)
        {
            picture.Image = null;
            if(Canvas!= null)Canvas.Dispose();
            ClearHistory();
            icons.Dispose();
        }
        base.Dispose(disposing);
    }
}
class Task1
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Editor());

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


