/* ЛР 3, часть 2, вариант 6. Запускать после ЛР 2.
   Размеры файлов заданы для новой учебной БД. Если журнал нужно перенести
   на другой диск, это делают отдельно после настройки пути и остановки БД. */
USE master;
GO
ALTER DATABASE UniversityTraining_V6
MODIFY FILE (NAME = N'UniversityTraining_V6',
             SIZE = 30MB, MAXSIZE = 50MB, FILEGROWTH = 5MB);
GO
ALTER DATABASE UniversityTraining_V6
MODIFY FILE (NAME = N'UniversityTraining_V6_log',
             SIZE = 10MB, MAXSIZE = 30MB, FILEGROWTH = 5MB);
GO
USE UniversityTraining_V6;
GO

-- Сначала ключи, затем связи: в ЛР 2 таблицы создавались без них.
ALTER TABLE dbo.Department ADD CONSTRAINT PK_Department PRIMARY KEY (DepartmentId);
ALTER TABLE dbo.Employee ADD CONSTRAINT PK_Employee PRIMARY KEY (EmployeeId);
ALTER TABLE dbo.Course ADD CONSTRAINT PK_Course PRIMARY KEY (CourseId);
ALTER TABLE dbo.Enrollment ADD CONSTRAINT PK_Enrollment PRIMARY KEY (EnrollmentId);
ALTER TABLE dbo.Certificate ADD CONSTRAINT PK_Certificate PRIMARY KEY (CertificateId);
GO
ALTER TABLE dbo.Employee ADD CONSTRAINT FK_Employee_Department
    FOREIGN KEY (DepartmentId) REFERENCES dbo.Department(DepartmentId);
ALTER TABLE dbo.Course ADD CONSTRAINT FK_Course_Teacher
    FOREIGN KEY (TeacherId) REFERENCES dbo.Employee(EmployeeId);
ALTER TABLE dbo.Enrollment ADD CONSTRAINT FK_Enrollment_Course
    FOREIGN KEY (CourseId) REFERENCES dbo.Course(CourseId);
ALTER TABLE dbo.Enrollment ADD CONSTRAINT FK_Enrollment_Employee
    FOREIGN KEY (EmployeeId) REFERENCES dbo.Employee(EmployeeId);
ALTER TABLE dbo.Certificate ADD CONSTRAINT FK_Certificate_Enrollment
    FOREIGN KEY (EnrollmentId) REFERENCES dbo.Enrollment(EnrollmentId);
GO

ALTER TABLE dbo.Department ADD CONSTRAINT UQ_Department_Name UNIQUE (DepartmentName);
ALTER TABLE dbo.Employee ADD CONSTRAINT UQ_Employee_Email UNIQUE (Email);
ALTER TABLE dbo.Enrollment ADD CONSTRAINT UQ_Enrollment_Pair UNIQUE (CourseId,EmployeeId);
ALTER TABLE dbo.Certificate ADD CONSTRAINT UQ_Certificate_Number UNIQUE (CertificateNumber);
ALTER TABLE dbo.Certificate ADD CONSTRAINT UQ_Certificate_Enrollment UNIQUE (EnrollmentId);
GO

ALTER TABLE dbo.Department ADD CONSTRAINT DF_Department_Active DEFAULT (1) FOR IsActive;
ALTER TABLE dbo.Employee ADD CONSTRAINT DF_Employee_Active DEFAULT (1) FOR IsActive;
ALTER TABLE dbo.Course ADD CONSTRAINT DF_Course_Status DEFAULT (N'Открыт') FOR Status;
ALTER TABLE dbo.Enrollment ADD CONSTRAINT DF_Enrollment_Date DEFAULT (CONVERT(date, GETDATE())) FOR EnrolledAt;
ALTER TABLE dbo.Enrollment ADD CONSTRAINT DF_Enrollment_Result DEFAULT (N'Записан') FOR Result;
ALTER TABLE dbo.Enrollment ADD CONSTRAINT DF_Enrollment_Completed DEFAULT (0) FOR IsCompleted;
ALTER TABLE dbo.Certificate ADD CONSTRAINT DF_Certificate_Valid DEFAULT (1) FOR IsValid;
GO

ALTER TABLE dbo.Course ADD CONSTRAINT CK_Course_Dates CHECK (EndDate >= StartDate);
ALTER TABLE dbo.Course ADD CONSTRAINT CK_Course_Hours CHECK (Hours > 0);
ALTER TABLE dbo.Course ADD CONSTRAINT CK_Course_Capacity CHECK (MaxParticipants > 0);
ALTER TABLE dbo.Course ADD CONSTRAINT CK_Course_Format
    CHECK (Format IN (N'Очная',N'Онлайн',N'Смешанная'));
ALTER TABLE dbo.Course ADD CONSTRAINT CK_Course_Status
    CHECK (Status IN (N'Открыт',N'Завершён',N'Отменён'));
ALTER TABLE dbo.Enrollment ADD CONSTRAINT CK_Enrollment_Attendance
    CHECK (AttendancePercent BETWEEN 0 AND 100);
ALTER TABLE dbo.Enrollment ADD CONSTRAINT CK_Enrollment_Score
    CHECK (FinalScore BETWEEN 0 AND 100);
ALTER TABLE dbo.Certificate ADD CONSTRAINT CK_Certificate_Hours CHECK (Hours > 0);
GO

-- Проверяем ограничения и параметры файлов.
SELECT name, size / 128.0 AS SizeMB,
       CASE WHEN max_size = -1 THEN NULL ELSE max_size / 128.0 END AS MaxSizeMB,
       CASE WHEN is_percent_growth = 1 THEN growth ELSE growth / 128.0 END AS Growth,
       physical_name
FROM sys.database_files;

SELECT OBJECT_NAME(parent_object_id) AS TableName, name AS ConstraintName, type_desc
FROM sys.objects
WHERE type IN ('PK','F','UQ','C','D')
ORDER BY TableName, type_desc, name;
GO
