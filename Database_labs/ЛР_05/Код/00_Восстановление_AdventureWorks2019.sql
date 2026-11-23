/* Подготовка к ЛР 5–7.
   1. Скачать AdventureWorks2019.bak из Microsoft SQL Server Samples.
   2. Положить его в папку, доступную службе SQL Server.
   3. Заменить путь в @BackupFile и запустить в SSMS.
   Скрипт не перезаписывает уже восстановленную базу. */
USE master;
GO
DECLARE @BackupFile nvarchar(4000) = N'C:\SQLBackups\AdventureWorks2019.bak';
IF DB_ID(N'AdventureWorks2019') IS NOT NULL
BEGIN
    PRINT N'AdventureWorks2019 уже существует; восстановление пропущено.';
END
ELSE
BEGIN
    DECLARE @DataPath nvarchar(4000) =
        CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultDataPath'));
    DECLARE @LogPath nvarchar(4000) =
        CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultLogPath'));
    IF @DataPath IS NULL OR @LogPath IS NULL
        THROW 50010,N'Не удалось узнать каталог DATA/LOG экземпляра SQL Server',1;
    DECLARE @Sql nvarchar(max) =
        N'RESTORE DATABASE AdventureWorks2019 FROM DISK = N''' +
        REPLACE(@BackupFile,N'''',N'''''') + N'''
        WITH MOVE N''AdventureWorks2019'' TO N''' +
        REPLACE(@DataPath+N'AdventureWorks2019.mdf',N'''',N'''''') +
        N''', MOVE N''AdventureWorks2019_log'' TO N''' +
        REPLACE(@LogPath+N'AdventureWorks2019_log.ldf',N'''',N'''''') +
        N''', STATS = 10;';
    EXEC sys.sp_executesql @Sql;
END;
GO
