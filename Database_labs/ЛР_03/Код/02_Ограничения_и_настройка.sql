/* Лабораторная работа 3. Создание БД и таблиц с ограничениями целостности. */

USE master;
GO

ALTER DATABASE Conference_Kashapov
MODIFY FILE (NAME = N'Conference_Kashapov', FILEGROWTH = 5MB);
GO

ALTER DATABASE Conference_Kashapov
MODIFY FILE (NAME = N'Conference_Kashapov', SIZE = 30MB, MAXSIZE = 50MB);
GO

ALTER DATABASE Conference_Kashapov
MODIFY FILE (NAME = N'Conference_Kashapov_log', FILEGROWTH = 5MB);
GO

ALTER DATABASE Conference_Kashapov
MODIFY FILE (NAME = N'Conference_Kashapov_log', SIZE = 10MB, MAXSIZE = 30MB);
GO

USE Conference_Kashapov;
GO

ALTER TABLE dbo.Participant
ADD CONSTRAINT CK_Participant_ParticipationForm
CHECK (ParticipationForm IN (N'Очная', N'Дистанционная'));
GO

ALTER TABLE dbo.Article
ADD CONSTRAINT CK_Article_Status
CHECK (Status IN (N'Принята', N'На рассмотрении', N'Отклонена'));
GO

ALTER TABLE dbo.Session
ADD CONSTRAINT CK_Session_WorkingHours
CHECK (StartTime >= '09:00' AND EndTime <= '17:00');
GO

SELECT name AS Имя_файла,
       size / 128.0 AS Размер_МБ,
       max_size / 128.0 AS Максимум_МБ,
       growth / 128.0 AS Автоприрост_МБ
FROM sys.database_files;
GO

SELECT name AS Таблица, type_desc AS Тип_ограничения
FROM sys.objects
WHERE type IN ('PK', 'F', 'C', 'UQ')
ORDER BY name;
GO

