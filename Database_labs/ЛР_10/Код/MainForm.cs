using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace TrainingApp
{
    public sealed class MainForm : Form
    {
        private readonly DataGridView grid = new DataGridView();
        private readonly TextBox search = new TextBox();
        private readonly ComboBox status = new ComboBox();
        private readonly ComboBox employee = new ComboBox();
        private readonly ComboBox course = new ComboBox();
        private readonly Label info = new Label();

        private static string ConnectionString
        {
            get
            {
                string configured = Environment.GetEnvironmentVariable("TRAINING_SQL_CONNECTION");
                return String.IsNullOrWhiteSpace(configured)
                    ? @"Data Source=.\SQLEXPRESS;Initial Catalog=UniversityTraining_V6;Integrated Security=True"
                    : configured;
            }
        }

        public MainForm()
        {
            Text = "Повышение квалификации сотрудников";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1000, 620);

            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 56, AutoScroll = true };
            top.Controls.Add(new Label { Text = "Название курса:", AutoSize = true, Padding = new Padding(5, 15, 0, 0) });
            search.Width = 180;
            top.Controls.Add(search);
            top.Controls.Add(new Label { Text = "Статус:", AutoSize = true, Padding = new Padding(12, 15, 0, 0) });
            status.DropDownStyle = ComboBoxStyle.DropDownList;
            status.Items.AddRange(new object[] { "Все", "Открыт", "Завершён", "Отменён" });
            status.SelectedIndex = 0;
            top.Controls.Add(status);
            var refresh = new Button { Text = "Обновить", AutoSize = true };
            refresh.Click += (sender, args) => RefreshAll();
            top.Controls.Add(refresh);

            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 85, AutoScroll = true };
            bottom.Controls.Add(new Label { Text = "Сотрудник:", AutoSize = true, Padding = new Padding(5, 15, 0, 0) });
            employee.Width = 210;
            employee.DropDownStyle = ComboBoxStyle.DropDownList;
            bottom.Controls.Add(employee);
            bottom.Controls.Add(new Label { Text = "Курс:", AutoSize = true, Padding = new Padding(10, 15, 0, 0) });
            course.Width = 240;
            course.DropDownStyle = ComboBoxStyle.DropDownList;
            bottom.Controls.Add(course);
            var enroll = new Button { Text = "Записать на курс", AutoSize = true };
            enroll.Click += (sender, args) => AddEnrollment();
            bottom.Controls.Add(enroll);
            info.AutoSize = true;
            info.Padding = new Padding(6, 15, 0, 0);
            bottom.Controls.Add(info);

            Controls.Add(grid);
            Controls.Add(bottom);
            Controls.Add(top);
            search.TextChanged += (sender, args) => LoadCourses();
            status.SelectedIndexChanged += (sender, args) => LoadCourses();
            Load += (sender, args) => RefreshAll();
        }

        private void RefreshAll()
        {
            LoadCourses();
            LoadChoices();
        }

        private void LoadCourses()
        {
            const string sql = @"
SELECT c.CourseId, c.CourseName, c.StartDate, c.EndDate, c.Hours,
       t.LastName + N' ' + t.FirstName AS Teacher, c.Format, c.Status,
       c.MaxParticipants - (SELECT COUNT(*) FROM dbo.Enrollment AS en
                            WHERE en.CourseId = c.CourseId AND en.Result <> N'Отменён') AS FreePlaces
FROM dbo.Course AS c
JOIN dbo.Employee AS t ON t.EmployeeId = c.TeacherId
WHERE c.CourseName LIKE N'%' + @Search + N'%'
  AND (@Status = N'Все' OR c.Status = @Status)
ORDER BY c.StartDate, c.CourseName;";
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                using (var command = new SqlCommand(sql, connection))
                using (var adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.Add("@Search", SqlDbType.NVarChar, 150).Value = search.Text.Trim();
                    command.Parameters.Add("@Status", SqlDbType.NVarChar, 30).Value = status.SelectedItem ?? "Все";
                    var table = new DataTable();
                    adapter.Fill(table);
                    grid.DataSource = table;
                    info.Text = "Курсов: " + table.Rows.Count;
                }
            }
            catch (SqlException ex)
            {
                info.Text = "Не удалось загрузить курсы";
                MessageBox.Show(ex.Message, "Подключение к базе", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChoices()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var employees = new DataTable();
                    using (var adapter = new SqlDataAdapter(
                        "SELECT EmployeeId, LastName + N' ' + FirstName AS DisplayName FROM dbo.Employee WHERE IsActive=1 ORDER BY LastName",
                        connection))
                        adapter.Fill(employees);
                    employee.DataSource = employees;
                    employee.DisplayMember = "DisplayName";
                    employee.ValueMember = "EmployeeId";

                    var courses = new DataTable();
                    using (var adapter = new SqlDataAdapter(
                        "SELECT CourseId, CourseName FROM dbo.Course WHERE Status=N'Открыт' ORDER BY StartDate",
                        connection))
                        adapter.Fill(courses);
                    course.DataSource = courses;
                    course.DisplayMember = "CourseName";
                    course.ValueMember = "CourseId";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Списки сотрудников и курсов", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddEnrollment()
        {
            if (employee.SelectedValue == null || course.SelectedValue == null) return;
            int employeeId = Convert.ToInt32(employee.SelectedValue);
            int courseId = Convert.ToInt32(course.SelectedValue);
            const string sql = @"
DECLARE @capacity INT;
SELECT @capacity=MaxParticipants FROM dbo.Course WITH (UPDLOCK,HOLDLOCK)
WHERE CourseId=@CourseId AND Status=N'Открыт';
IF @capacity IS NULL THROW 50001,N'Курс закрыт или не найден',1;
IF EXISTS (SELECT 1 FROM dbo.Enrollment WHERE CourseId=@CourseId AND EmployeeId=@EmployeeId)
    THROW 50002,N'Сотрудник уже записан на курс',1;
IF (SELECT COUNT(*) FROM dbo.Enrollment WHERE CourseId=@CourseId AND Result<>N'Отменён')>=@capacity
    THROW 50003,N'Свободных мест нет',1;
DECLARE @id INT;
SELECT @id=ISNULL(MAX(EnrollmentId),0)+1 FROM dbo.Enrollment WITH (UPDLOCK,HOLDLOCK);
INSERT dbo.Enrollment(EnrollmentId,CourseId,EmployeeId,EnrolledAt,Result,PaymentSource,IsCompleted)
VALUES(@id,@CourseId,@EmployeeId,CONVERT(date,GETDATE()),N'Записан',N'Университет',0);
SELECT @id;";
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                    using (var command = new SqlCommand(sql, connection, transaction))
                    {
                        command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                        command.Parameters.Add("@CourseId", SqlDbType.Int).Value = courseId;
                        try
                        {
                            int id = Convert.ToInt32(command.ExecuteScalar());
                            transaction.Commit();
                            MessageBox.Show("Сотрудник записан. Номер записи: " + id, "Готово");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                RefreshAll();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Запись на курс", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
