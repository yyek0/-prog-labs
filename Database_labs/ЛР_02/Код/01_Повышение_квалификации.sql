/* ЛР 2, часть 2, вариант 6. Повышение квалификации сотрудников университета.
   Запускать в SSMS целиком. ЛР 3 добавит ограничения PK, FK, CHECK и DEFAULT. */
USE master;
GO
IF DB_ID(N'UniversityTraining_V6') IS NULL
    CREATE DATABASE UniversityTraining_V6;
GO
USE UniversityTraining_V6;
GO

CREATE TABLE dbo.Department
(
    DepartmentId INT NOT NULL,
    DepartmentName NVARCHAR(100) NOT NULL,
    Faculty NVARCHAR(100) NOT NULL,
    Building NVARCHAR(50) NULL,
    OfficeNumber NVARCHAR(15) NULL,
    Phone NVARCHAR(30) NULL,
    Email NVARCHAR(120) NULL,
    HeadName NVARCHAR(120) NULL,
    CreatedDate DATE NULL,
    IsActive BIT NULL
);

CREATE TABLE dbo.Employee
(
    EmployeeId INT NOT NULL,
    DepartmentId INT NOT NULL,
    LastName NVARCHAR(60) NOT NULL,
    FirstName NVARCHAR(60) NOT NULL,
    MiddleName NVARCHAR(60) NULL,
    JobTitle NVARCHAR(100) NOT NULL,
    HireDate DATE NOT NULL,
    Email NVARCHAR(120) NOT NULL,
    Phone NVARCHAR(30) NULL,
    AcademicDegree NVARCHAR(80) NULL,
    IsActive BIT NULL
);

CREATE TABLE dbo.Course
(
    CourseId INT NOT NULL,
    TeacherId INT NOT NULL,
    CourseName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Hours INT NOT NULL,
    MaxParticipants INT NOT NULL,
    Location NVARCHAR(120) NULL,
    Format NVARCHAR(30) NOT NULL,
    Status NVARCHAR(30) NULL
);

CREATE TABLE dbo.Enrollment
(
    EnrollmentId INT NOT NULL,
    CourseId INT NOT NULL,
    EmployeeId INT NOT NULL,
    EnrolledAt DATE NOT NULL,
    AttendancePercent DECIMAL(5,2) NULL,
    FinalScore DECIMAL(5,2) NULL,
    Result NVARCHAR(30) NULL,
    PaymentSource NVARCHAR(60) NULL,
    Notes NVARCHAR(300) NULL,
    IsCompleted BIT NULL
);

CREATE TABLE dbo.Certificate
(
    CertificateId INT NOT NULL,
    EnrollmentId INT NOT NULL,
    CertificateNumber NVARCHAR(30) NOT NULL,
    IssueDate DATE NOT NULL,
    Qualification NVARCHAR(150) NOT NULL,
    Hours INT NOT NULL,
    IssuedBy NVARCHAR(120) NOT NULL,
    Registrar NVARCHAR(120) NULL,
    IsValid BIT NULL,
    Remarks NVARCHAR(300) NULL
);
GO

INSERT dbo.Department VALUES
(1,N'Кафедра информатики',N'Физико-математический факультет',N'Корпус 1',N'205',N'8-34784-20001',N'it@example.edu',N'Павлова И. А.','2020-09-01',1),
(2,N'Кафедра математики',N'Физико-математический факультет',N'Корпус 1',N'207',N'8-34784-20002',N'math@example.edu',N'Орлов П. В.','2020-09-01',1),
(3,N'Кафедра педагогики',N'Педагогический факультет',N'Корпус 2',N'110',N'8-34784-20003',N'ped@example.edu',N'Громова Е. С.','2020-09-01',1);

INSERT dbo.Employee VALUES
(1,1,N'Павлова',N'Ирина',N'Андреевна',N'Доцент','2018-09-01',N'pavlova@example.edu',NULL,N'Кандидат наук',1),
(2,1,N'Кашапов',N'Роман',N'Радикович',N'Преподаватель','2022-09-01',N'kashapov@example.edu',N'8-900-100-00-01',NULL,1),
(3,2,N'Орлов',N'Павел',N'Викторович',N'Профессор','2010-09-01',N'orlov@example.edu',NULL,N'Доктор наук',1),
(4,2,N'Смирнова',N'Анна',N'Петровна',N'Ассистент','2023-09-01',N'smirnova@example.edu',NULL,NULL,1),
(5,3,N'Громова',N'Елена',N'Сергеевна',N'Доцент','2016-09-01',N'gromova@example.edu',NULL,N'Кандидат наук',1),
(6,3,N'Иванов',N'Денис',N'Олегович',N'Преподаватель','2021-09-01',N'ivanov@example.edu',NULL,NULL,1);

INSERT dbo.Course VALUES
(1,1,N'Основы SQL Server',N'Таблицы, ключи и запросы','2026-10-05','2026-10-30',72,12,N'Корпус 1, 205',N'Очная',N'Завершён'),
(2,3,N'Математические методы в обучении',N'Прикладные задачи и методика','2026-11-10','2026-12-05',36,15,N'Корпус 1, 207',N'Очная',N'Открыт'),
(3,5,N'Цифровые инструменты преподавателя',N'Работа с цифровой средой','2026-11-15','2026-12-15',48,20,N'Дистанционно',N'Онлайн',N'Открыт'),
(4,1,N'Анализ данных в образовании',N'Обработка учебных данных','2027-02-01','2027-03-01',54,10,N'Корпус 1, 205',N'Смешанная',N'Открыт');

INSERT dbo.Enrollment VALUES
(1,1,2,'2026-09-25',100,92,N'Сдано',N'Университет',NULL,1),
(2,1,4,'2026-09-26',95,85,N'Сдано',N'Университет',NULL,1),
(3,1,6,'2026-09-27',88,77,N'Сдано',N'Университет',NULL,1),
(4,2,1,'2026-11-01',NULL,NULL,N'В процессе',N'Университет',NULL,0),
(5,2,2,'2026-11-02',NULL,NULL,N'В процессе',N'Университет',NULL,0),
(6,3,3,'2026-11-03',NULL,NULL,N'В процессе',N'Грант',NULL,0),
(7,3,4,'2026-11-04',NULL,NULL,N'В процессе',N'Университет',NULL,0),
(8,4,5,'2026-11-05',NULL,NULL,N'Записан',N'Университет',NULL,0);

INSERT dbo.Certificate VALUES
(1,1,N'ПК-2026-001','2026-10-31',N'Работа с базами данных',72,N'Учебный отдел',N'Петрова М. О.',1,NULL),
(2,2,N'ПК-2026-002','2026-10-31',N'Работа с базами данных',72,N'Учебный отдел',N'Петрова М. О.',1,NULL),
(3,3,N'ПК-2026-003','2026-10-31',N'Работа с базами данных',72,N'Учебный отдел',N'Петрова М. О.',1,NULL);
GO

-- Контроль наполнения: 3 кафедры, 6 сотрудников, 4 курса,
-- 8 записей на курсы и 3 выданных свидетельства.
SELECT N'Department' AS TableName, COUNT(*) AS RowsCount FROM dbo.Department
UNION ALL SELECT N'Employee', COUNT(*) FROM dbo.Employee
UNION ALL SELECT N'Course', COUNT(*) FROM dbo.Course
UNION ALL SELECT N'Enrollment', COUNT(*) FROM dbo.Enrollment
UNION ALL SELECT N'Certificate', COUNT(*) FROM dbo.Certificate;
GO
