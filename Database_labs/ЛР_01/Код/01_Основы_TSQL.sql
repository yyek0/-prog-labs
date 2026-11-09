/* Лабораторная работа 1. Введение в MS SQL Server и T-SQL.
   Примеры DDL-команд с авторским именем базы и таблицы. */

IF DB_ID(N'Kashapov_Lab01') IS NULL
BEGIN
    CREATE DATABASE Kashapov_Lab01;
END;
GO

USE Kashapov_Lab01;
GO

CREATE TABLE Kashapov_Research
(
    ResearchId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Topic NVARCHAR(120) NOT NULL,
    CreatedAt DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    IsActive BIT NOT NULL DEFAULT 1
);
GO

INSERT INTO Kashapov_Research (Topic)
VALUES
    (N'Изучение типов данных T-SQL'),
    (N'Создание таблиц и ограничений');
GO

SELECT ResearchId, Topic, CreatedAt, IsActive
FROM Kashapov_Research;
GO

EXEC sp_rename N'dbo.Kashapov_Research', N'Kashapov_ResearchResults';
GO

SELECT *
FROM dbo.Kashapov_ResearchResults;
GO

DROP TABLE dbo.Kashapov_ResearchResults;
GO

USE master;
GO

DROP DATABASE Kashapov_Lab01;
GO
