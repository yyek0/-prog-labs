/*
  Подготовка учебной базы для ЛР 5–7.
  1. Скачать AdventureWorks2019.bak по официальной ссылке Microsoft:
     https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks2019.bak
  2. Поместить файл в папку Backup экземпляра SQL Server.
  3. При необходимости изменить пути в командах MOVE.
*/

USE master;
GO

RESTORE DATABASE AdventureWorks2019
FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\Backup\AdventureWorks2019.bak'
WITH
    MOVE N'AdventureWorks2019' TO N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\AdventureWorks2019.mdf',
    MOVE N'AdventureWorks2019_log' TO N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\AdventureWorks2019_log.ldf',
    STATS = 10;
GO
