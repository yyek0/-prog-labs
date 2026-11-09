using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FurnitureApp
{
    public sealed class MainForm : Form
    {
        private const string ConnectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=Furniture_Kashapov;Integrated Security=True";

        private readonly DataGridView furnitureGrid;
        private readonly Button refreshButton;
        private readonly Label statusLabel;

        public MainForm()
        {
            Text = "Мебель — список товаров";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 500);

            furnitureGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            refreshButton = new Button
            {
                Text = "Обновить список",
                AutoSize = true,
                Margin = new Padding(12)
            };
            refreshButton.Click += RefreshButton_Click;

            statusLabel = new Label
            {
                AutoSize = true,
                Text = "Готово к загрузке данных.",
                Padding = new Padding(8, 15, 0, 0)
            };

            var topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 56,
                FlowDirection = FlowDirection.LeftToRight
            };
            topPanel.Controls.Add(refreshButton);
            topPanel.Controls.Add(statusLabel);

            Controls.Add(furnitureGrid);
            Controls.Add(topPanel);

            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadFurniture();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadFurniture();
        }

        private void LoadFurniture()
        {
            const string query = "SELECT * FROM dbo.ListFurniture ORDER BY CategoryName, FurnitureName;";

            using (var connection = new SqlConnection(ConnectionString))
            using (var adapter = new SqlDataAdapter(query, connection))
            {
                var table = new DataTable();
                adapter.Fill(table);
                furnitureGrid.DataSource = table;
                statusLabel.Text = "Загружено записей: " + table.Rows.Count;
            }
        }
    }
}
