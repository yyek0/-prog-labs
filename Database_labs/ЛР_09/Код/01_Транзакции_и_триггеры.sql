/* ЛР 9, вариант 6. Основа — база курсов из ЛР 3.
   SQL Server не поддерживает триггер на SELECT: чтение и запись журнала
   объединяем явной транзакцией. Два триггера покрывают INSERT, UPDATE, DELETE. */
USE UniversityTraining_V6;
GO
CREATE TABLE dbo.TrainingAudit
(
    AuditId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ActionName NVARCHAR(20) NOT NULL,
    EnrollmentId INT NULL,
    CourseId INT NULL,
    OldResult NVARCHAR(30) NULL,
    NewResult NVARCHAR(30) NULL,
    Note NVARCHAR(250) NULL,
    ChangedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

-- Триггер 1: срабатывает при вставке одной или нескольких записей.
CREATE OR ALTER TRIGGER dbo.tr_Enrollment_Insert
ON dbo.Enrollment AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.TrainingAudit(ActionName,EnrollmentId,CourseId,NewResult,Note)
    SELECT N'INSERT',i.EnrollmentId,i.CourseId,i.Result,N'Запись сотрудника на курс'
    FROM inserted AS i;
END;
GO

-- Триггер 2: для UPDATE и DELETE; использует обе псевдотаблицы.
CREATE OR ALTER TRIGGER dbo.tr_Enrollment_Change
ON dbo.Enrollment AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.TrainingAudit(ActionName,EnrollmentId,CourseId,
                             OldResult,NewResult,Note)
    SELECT CASE WHEN i.EnrollmentId IS NULL THEN N'DELETE' ELSE N'UPDATE' END,
           COALESCE(i.EnrollmentId,d.EnrollmentId),
           COALESCE(i.CourseId,d.CourseId),
           d.Result,i.Result,
           CASE WHEN i.EnrollmentId IS NULL
                THEN N'Удалена запись о курсе' ELSE N'Изменена запись о курсе' END
    FROM inserted AS i
    FULL JOIN deleted AS d ON d.EnrollmentId=i.EnrollmentId;
END;
GO

-- Транзакция 1: выводим сотрудников курса и сохраняем число строк;
-- обе операции фиксируются вместе.
BEGIN TRANSACTION;
SELECT e.EmployeeId,e.LastName,e.FirstName,en.Result
FROM dbo.Enrollment AS en
JOIN dbo.Employee AS e ON e.EmployeeId=en.EmployeeId
WHERE en.CourseId=1
ORDER BY e.LastName;
INSERT dbo.TrainingAudit(ActionName,CourseId,Note)
SELECT N'SELECT',1,
       N'Просмотрены записи курса 1: '+CONVERT(nvarchar(20),COUNT(*))
FROM dbo.Enrollment WHERE CourseId=1;
COMMIT TRANSACTION;
GO

-- Транзакция 2: аналогичную запись отменяем; число записей до/после одинаково.
SELECT COUNT(*) AS AuditBeforeRollback FROM dbo.TrainingAudit;
BEGIN TRANSACTION;
SELECT e.EmployeeId,e.LastName,en.Result
FROM dbo.Enrollment AS en
JOIN dbo.Employee AS e ON e.EmployeeId=en.EmployeeId
WHERE en.CourseId=2;
INSERT dbo.TrainingAudit(ActionName,CourseId,Note)
SELECT N'SELECT',2,
       N'Просмотрены записи курса 2: '+CONVERT(nvarchar(20),COUNT(*))
FROM dbo.Enrollment WHERE CourseId=2;
SELECT COUNT(*) AS AuditInsideTransaction FROM dbo.TrainingAudit;
ROLLBACK TRANSACTION;
SELECT COUNT(*) AS AuditAfterRollback FROM dbo.TrainingAudit;
GO

-- Проверка трёх событий. После DELETE учебная запись не остаётся,
-- а журнал INSERT/UPDATE/DELETE сохраняется.
BEGIN TRANSACTION;
DECLARE @Id INT;
SELECT @Id=ISNULL(MAX(EnrollmentId),0)+1
FROM dbo.Enrollment WITH (UPDLOCK,HOLDLOCK);
INSERT dbo.Enrollment(EnrollmentId,CourseId,EmployeeId,EnrolledAt,Result,IsCompleted)
VALUES(@Id,4,6,CONVERT(date,GETDATE()),N'Записан',0);
UPDATE dbo.Enrollment SET Result=N'В процессе' WHERE EnrollmentId=@Id;
DELETE dbo.Enrollment WHERE EnrollmentId=@Id;
COMMIT TRANSACTION;
SELECT * FROM dbo.TrainingAudit ORDER BY AuditId;
GO
