/* ЛР 1. Основные команды T-SQL. Для этой работы индивидуальный вариант
   в предоставленных материалах не указан. Данные остаются после запуска. */
USE master;
GO
IF DB_ID(N'TSQL_Intro_V6') IS NULL CREATE DATABASE TSQL_Intro_V6;
GO
USE TSQL_Intro_V6;
GO
IF OBJECT_ID(N'dbo.ResearchTasks',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ResearchTasks
    (
        TaskId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Topic NVARCHAR(120) NOT NULL,
        AuthorName NVARCHAR(100) NOT NULL,
        StartDate DATE NOT NULL,
        Deadline DATE NULL,
        Priority INT NOT NULL,
        HoursPlanned DECIMAL(6,2) NOT NULL,
        HoursSpent DECIMAL(6,2) NOT NULL DEFAULT 0,
        IsCompleted BIT NOT NULL DEFAULT 0,
        Note NVARCHAR(250) NULL,
        CONSTRAINT CK_ResearchTasks_Priority CHECK(Priority BETWEEN 1 AND 5)
    );
END;
GO
IF NOT EXISTS (SELECT 1 FROM dbo.ResearchTasks)
BEGIN
    INSERT dbo.ResearchTasks
        (Topic,AuthorName,StartDate,Deadline,Priority,HoursPlanned,Note)
    VALUES
        (N'Типы данных SQL Server',N'Студент','2026-09-01','2026-09-10',2,4,N'Числа, строки и даты'),
        (N'Создание таблиц',N'Студент','2026-09-03','2026-09-15',1,6,N'Первичный ключ и ограничения'),
        (N'Запросы SELECT',N'Студент','2026-09-05','2026-09-20',3,5,NULL);
END;
GO
SELECT TaskId,Topic,Priority,HoursPlanned,IsCompleted
FROM dbo.ResearchTasks ORDER BY Priority,TaskId;

-- Изменяем одну запись и проверяем результат.
UPDATE dbo.ResearchTasks
SET HoursSpent=2,IsCompleted=1
WHERE Topic=N'Типы данных SQL Server';
SELECT TaskId,Topic,HoursSpent,IsCompleted
FROM dbo.ResearchTasks WHERE IsCompleted=1;
GO
