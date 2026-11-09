/* Лабораторная работа 2. ER-модель «Научная конференция». Вариант 7. */

IF DB_ID(N'Conference_Kashapov') IS NULL
BEGIN
    CREATE DATABASE Conference_Kashapov;
END;
GO

USE Conference_Kashapov;
GO

CREATE TABLE dbo.Conference
(
    ConferenceId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    City NVARCHAR(80) NOT NULL,
    Organizer NVARCHAR(120) NOT NULL,
    ContactEmail NVARCHAR(120) NOT NULL,
    RegistrationDeadline DATE NOT NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Планируется',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT CK_Conference_Dates CHECK (EndDate >= StartDate)
);

CREATE TABLE dbo.Room
(
    RoomId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Building NVARCHAR(80) NOT NULL,
    RoomNumber NVARCHAR(20) NOT NULL,
    Capacity INT NOT NULL,
    HasProjector BIT NOT NULL DEFAULT 1,
    HasComputer BIT NOT NULL DEFAULT 1,
    FloorNumber INT NOT NULL,
    ResponsiblePerson NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(30) NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Room UNIQUE (Building, RoomNumber),
    CONSTRAINT CK_Room_Capacity CHECK (Capacity > 0)
);

CREATE TABLE dbo.Section
(
    SectionId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ConferenceId INT NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Chairperson NVARCHAR(120) NOT NULL,
    Description NVARCHAR(500) NULL,
    PlannedReports INT NOT NULL DEFAULT 0,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    BreakStart TIME NOT NULL DEFAULT '12:00',
    BreakEnd TIME NOT NULL DEFAULT '13:00',
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Section_Conference FOREIGN KEY (ConferenceId)
        REFERENCES dbo.Conference (ConferenceId),
    CONSTRAINT CK_Section_Time CHECK (EndTime > StartTime)
);

CREATE TABLE dbo.Participant
(
    ParticipantId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    LastName NVARCHAR(60) NOT NULL,
    FirstName NVARCHAR(60) NOT NULL,
    MiddleName NVARCHAR(60) NULL,
    Organization NVARCHAR(160) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    Email NVARCHAR(120) NOT NULL UNIQUE,
    Phone NVARCHAR(30) NULL,
    City NVARCHAR(80) NOT NULL,
    RegistrationDate DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    ParticipationForm NVARCHAR(30) NOT NULL DEFAULT N'Очная'
);

CREATE TABLE dbo.Article
(
    ArticleId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ConferenceId INT NOT NULL,
    ParticipantId INT NOT NULL,
    Title NVARCHAR(250) NOT NULL,
    PageCount INT NOT NULL,
    SubmittedAt DATE NOT NULL,
    FileName NVARCHAR(160) NOT NULL,
    AbstractText NVARCHAR(1000) NOT NULL,
    Keywords NVARCHAR(250) NOT NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Принята',
    CONSTRAINT FK_Article_Conference FOREIGN KEY (ConferenceId)
        REFERENCES dbo.Conference (ConferenceId),
    CONSTRAINT FK_Article_Participant FOREIGN KEY (ParticipantId)
        REFERENCES dbo.Participant (ParticipantId),
    CONSTRAINT CK_Article_Pages CHECK (PageCount BETWEEN 2 AND 30)
);

CREATE TABLE dbo.ArticleAuthor
(
    ArticleAuthorId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ArticleId INT NOT NULL,
    LastName NVARCHAR(60) NOT NULL,
    FirstName NVARCHAR(60) NOT NULL,
    MiddleName NVARCHAR(60) NULL,
    Organization NVARCHAR(160) NOT NULL,
    AuthorOrder TINYINT NOT NULL,
    Email NVARCHAR(120) NULL,
    IsCorresponding BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_ArticleAuthor_Article FOREIGN KEY (ArticleId)
        REFERENCES dbo.Article (ArticleId),
    CONSTRAINT UQ_ArticleAuthor_Order UNIQUE (ArticleId, AuthorOrder)
);

CREATE TABLE dbo.Presentation
(
    PresentationId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ArticleId INT NOT NULL UNIQUE,
    Title NVARCHAR(250) NOT NULL,
    FileName NVARCHAR(160) NOT NULL,
    SlideCount INT NOT NULL,
    SubmittedAt DATE NOT NULL,
    Format NVARCHAR(20) NOT NULL DEFAULT N'PPTX',
    DurationMinutes INT NOT NULL DEFAULT 30,
    IsApproved BIT NOT NULL DEFAULT 1,
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_Presentation_Article FOREIGN KEY (ArticleId)
        REFERENCES dbo.Article (ArticleId),
    CONSTRAINT CK_Presentation_Slides CHECK (SlideCount > 0)
);

CREATE TABLE dbo.Session
(
    SessionId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    SectionId INT NOT NULL,
    RoomId INT NOT NULL,
    SessionDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Moderator NVARCHAR(120) NOT NULL,
    SessionType NVARCHAR(40) NOT NULL DEFAULT N'Секционное заседание',
    Status NVARCHAR(30) NOT NULL DEFAULT N'Запланировано',
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_Session_Section FOREIGN KEY (SectionId)
        REFERENCES dbo.Section (SectionId),
    CONSTRAINT FK_Session_Room FOREIGN KEY (RoomId)
        REFERENCES dbo.Room (RoomId),
    CONSTRAINT CK_Session_Time CHECK (EndTime > StartTime)
);

CREATE TABLE dbo.SessionReport
(
    SessionReportId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    SessionId INT NOT NULL,
    ArticleId INT NOT NULL,
    PresentationId INT NOT NULL,
    ReportOrder TINYINT NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Speaker NVARCHAR(120) NOT NULL,
    AttendanceCount INT NULL,
    Result NVARCHAR(250) NULL,
    CONSTRAINT FK_SessionReport_Session FOREIGN KEY (SessionId)
        REFERENCES dbo.Session (SessionId),
    CONSTRAINT FK_SessionReport_Article FOREIGN KEY (ArticleId)
        REFERENCES dbo.Article (ArticleId),
    CONSTRAINT FK_SessionReport_Presentation FOREIGN KEY (PresentationId)
        REFERENCES dbo.Presentation (PresentationId),
    CONSTRAINT UQ_SessionReport_Order UNIQUE (SessionId, ReportOrder),
    CONSTRAINT CK_SessionReport_Duration CHECK (DATEDIFF(MINUTE, StartTime, EndTime) = 30)
);
GO

INSERT INTO dbo.Conference
    (Name, StartDate, EndDate, City, Organizer, ContactEmail, RegistrationDeadline, Status)
VALUES
    (N'Молодежная наука 2026', '2026-05-20', '2026-05-22', N'Бирск',
     N'Бирский филиал УУНиТ', N'conference@birsksu.ru', '2026-05-01', N'Планируется');

INSERT INTO dbo.Room
    (Building, RoomNumber, Capacity, HasProjector, HasComputer, FloorNumber, ResponsiblePerson, Phone)
VALUES
    (N'Главный корпус', N'301', 40, 1, 1, 3, N'Иванова Елена Сергеевна', N'+7 34784 2-00-01'),
    (N'Главный корпус', N'305', 32, 1, 1, 3, N'Петрова Марина Олеговна', N'+7 34784 2-00-02'),
    (N'Корпус Б', N'101', 60, 1, 1, 1, N'Сидоров Андрей Викторович', N'+7 34784 2-00-03');

INSERT INTO dbo.Section
    (ConferenceId, Name, Chairperson, Description, PlannedReports, StartTime, EndTime)
VALUES
    (1, N'Информационные технологии', N'Профессор Андреев А. В.', N'Программирование и базы данных', 4, '09:00', '17:00'),
    (1, N'Естественные науки', N'Доцент Смирнова Н. П.', N'Исследования в области экологии', 3, '09:00', '17:00'),
    (1, N'Гуманитарные исследования', N'Доцент Кузнецова Е. И.', N'Язык и общество', 3, '09:00', '17:00');

INSERT INTO dbo.Participant
    (LastName, FirstName, MiddleName, Organization, Position, Email, Phone, City, ParticipationForm)
VALUES
    (N'Кашапов', N'Роман', N'Радикович', N'Бирский филиал УУНиТ', N'Студент', N'roman.kashapov@example.com', N'+7 900 100-00-01', N'Бирск', N'Очная'),
    (N'Васильева', N'Анна', N'Игоревна', N'УУНиТ', N'Магистрант', N'anna.vasilyeva@example.com', N'+7 900 100-00-02', N'Уфа', N'Очная'),
    (N'Морозов', N'Денис', N'Олегович', N'БГПУ', N'Аспирант', N'denis.morozov@example.com', N'+7 900 100-00-03', N'Уфа', N'Дистанционная'),
    (N'Соколова', N'Ирина', N'Павловна', N'Бирский филиал УУНиТ', N'Преподаватель', N'irina.sokolova@example.com', N'+7 900 100-00-04', N'Бирск', N'Очная');

INSERT INTO dbo.Article
    (ConferenceId, ParticipantId, Title, PageCount, SubmittedAt, FileName, AbstractText, Keywords)
VALUES
    (1, 1, N'Применение SQL Server в учебных проектах', 8, '2026-04-10', N'kashapov_sql.docx', N'Рассмотрены возможности SQL Server.', N'SQL Server, база данных, T-SQL'),
    (1, 2, N'Анализ данных средствами T-SQL', 10, '2026-04-11', N'vasilyeva_tsql.docx', N'Рассмотрены аналитические запросы.', N'T-SQL, анализ данных'),
    (1, 3, N'Цифровые методы экологического мониторинга', 7, '2026-04-12', N'morozov_ecology.docx', N'Описан сбор экологических данных.', N'экология, мониторинг'),
    (1, 4, N'Язык современной молодежи', 6, '2026-04-13', N'sokolova_language.docx', N'Исследованы языковые особенности.', N'язык, молодежь');

INSERT INTO dbo.ArticleAuthor
    (ArticleId, LastName, FirstName, MiddleName, Organization, AuthorOrder, Email, IsCorresponding)
VALUES
    (1, N'Кашапов', N'Роман', N'Радикович', N'Бирский филиал УУНиТ', 1, N'roman.kashapov@example.com', 1),
    (2, N'Васильева', N'Анна', N'Игоревна', N'УУНиТ', 1, N'anna.vasilyeva@example.com', 1),
    (3, N'Морозов', N'Денис', N'Олегович', N'БГПУ', 1, N'denis.morozov@example.com', 1),
    (4, N'Соколова', N'Ирина', N'Павловна', N'Бирский филиал УУНиТ', 1, N'irina.sokolova@example.com', 1);

INSERT INTO dbo.Presentation
    (ArticleId, Title, FileName, SlideCount, SubmittedAt, DurationMinutes, IsApproved)
VALUES
    (1, N'Применение SQL Server в учебных проектах', N'kashapov_sql.pptx', 12, '2026-04-10', 30, 1),
    (2, N'Анализ данных средствами T-SQL', N'vasilyeva_tsql.pptx', 14, '2026-04-11', 30, 1),
    (3, N'Цифровые методы экологического мониторинга', N'morozov_ecology.pptx', 10, '2026-04-12', 30, 1),
    (4, N'Язык современной молодежи', N'sokolova_language.pptx', 9, '2026-04-13', 30, 1);

INSERT INTO dbo.Session
    (SectionId, RoomId, SessionDate, StartTime, EndTime, Moderator)
VALUES
    (1, 1, '2026-05-20', '09:00', '11:00', N'Андреев А. В.'),
    (2, 2, '2026-05-20', '09:00', '10:30', N'Смирнова Н. П.'),
    (3, 3, '2026-05-20', '13:00', '14:30', N'Кузнецова Е. И.');

INSERT INTO dbo.SessionReport
    (SessionId, ArticleId, PresentationId, ReportOrder, StartTime, EndTime, Speaker, AttendanceCount, Result)
VALUES
    (1, 1, 1, 1, '09:00', '09:30', N'Кашапов Роман Радикович', 35, N'Доклад принят'),
    (1, 2, 2, 2, '09:30', '10:00', N'Васильева Анна Игоревна', 31, N'Доклад принят'),
    (2, 3, 3, 1, '09:00', '09:30', N'Морозов Денис Олегович', 28, N'Доклад принят'),
    (3, 4, 4, 1, '13:00', '13:30', N'Соколова Ирина Павловна', 42, N'Доклад принят');
GO

SELECT c.Name AS Конференция, s.Name AS Секция, r.Building, r.RoomNumber,
       sr.StartTime, sr.EndTime, a.Title AS Статья, sr.Speaker
FROM dbo.SessionReport AS sr
JOIN dbo.Session AS se ON se.SessionId = sr.SessionId
JOIN dbo.Section AS s ON s.SectionId = se.SectionId
JOIN dbo.Conference AS c ON c.ConferenceId = s.ConferenceId
JOIN dbo.Room AS r ON r.RoomId = se.RoomId
JOIN dbo.Article AS a ON a.ArticleId = sr.ArticleId
ORDER BY s.Name, sr.StartTime;
GO
