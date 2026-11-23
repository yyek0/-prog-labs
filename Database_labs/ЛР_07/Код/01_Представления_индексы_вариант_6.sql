/* ЛР 7, часть 2, вариант 6.
   Перед запуском восстановить AdventureWorks2019.
   Скрипт создаёт отдельную учебную БД, исходную базу не изменяет.
   Повторный запуск: создать новую пустую учебную БД или удалить только её. */
USE master;
GO
IF DB_ID(N'PersonViews_V6') IS NULL CREATE DATABASE PersonViews_V6;
GO
USE PersonViews_V6;
GO

-- Пять полных копий таблиц варианта. XML из Person конвертируется
-- в нетипизированный XML: привязка к схеме XML остаётся в исходной БД.
SELECT * INTO dbo.BusinessEntity_Copy
FROM AdventureWorks2019.Person.BusinessEntity;
SELECT * INTO dbo.BusinessEntityAddress_Copy
FROM AdventureWorks2019.Person.BusinessEntityAddress;
SELECT * INTO dbo.BusinessEntityContact_Copy
FROM AdventureWorks2019.Person.BusinessEntityContact;
SELECT BusinessEntityID, PersonType, NameStyle, Title, FirstName, MiddleName,
       LastName, Suffix, EmailPromotion,
       CONVERT(xml,AdditionalContactInfo) AS AdditionalContactInfo,
       CONVERT(xml,Demographics) AS Demographics,
       rowguid, ModifiedDate
INTO dbo.Person_Copy
FROM AdventureWorks2019.Person.Person;
SELECT * INTO dbo.Password_Copy
FROM AdventureWorks2019.Person.Password;
GO

SELECT N'BusinessEntity' AS TableName,COUNT(*) AS RowCount FROM dbo.BusinessEntity_Copy
UNION ALL SELECT N'BusinessEntityAddress',COUNT(*) FROM dbo.BusinessEntityAddress_Copy
UNION ALL SELECT N'BusinessEntityContact',COUNT(*) FROM dbo.BusinessEntityContact_Copy
UNION ALL SELECT N'Person',COUNT(*) FROM dbo.Person_Copy
UNION ALL SELECT N'Password',COUNT(*) FROM dbo.Password_Copy;
GO

-- Представление для каждой скопированной таблицы.
CREATE VIEW dbo.vBusinessEntity AS
SELECT BusinessEntityID,rowguid,ModifiedDate FROM dbo.BusinessEntity_Copy;
GO
CREATE VIEW dbo.vBusinessEntityAddress AS
SELECT BusinessEntityID,AddressID,AddressTypeID,ModifiedDate
FROM dbo.BusinessEntityAddress_Copy;
GO
CREATE VIEW dbo.vBusinessEntityContact AS
SELECT BusinessEntityID,PersonID,ContactTypeID,ModifiedDate
FROM dbo.BusinessEntityContact_Copy;
GO
CREATE VIEW dbo.vPerson AS
SELECT BusinessEntityID,PersonType,FirstName,LastName,ModifiedDate
FROM dbo.Person_Copy;
GO
CREATE VIEW dbo.vPassword AS
SELECT BusinessEntityID,ModifiedDate FROM dbo.Password_Copy;
GO

-- Представление из трёх таблиц: организация и её контактное лицо.
CREATE VIEW dbo.vEntityContacts AS
SELECT be.BusinessEntityID, bc.PersonID, p.FirstName,p.LastName,p.PersonType
FROM dbo.BusinessEntity_Copy AS be
JOIN dbo.BusinessEntityContact_Copy AS bc
  ON bc.BusinessEntityID=be.BusinessEntityID
JOIN dbo.Person_Copy AS p ON p.BusinessEntityID=bc.PersonID;
GO
-- Представление из пяти таблиц: добавлены адрес и запись пароля контакта.
CREATE VIEW dbo.vEntityContactDetails AS
SELECT be.BusinessEntityID, ba.AddressID, bc.PersonID,
       p.FirstName,p.LastName,p.PersonType,pw.ModifiedDate AS PasswordDate
FROM dbo.BusinessEntity_Copy AS be
JOIN dbo.BusinessEntityAddress_Copy AS ba
  ON ba.BusinessEntityID=be.BusinessEntityID
JOIN dbo.BusinessEntityContact_Copy AS bc
  ON bc.BusinessEntityID=be.BusinessEntityID
JOIN dbo.Person_Copy AS p ON p.BusinessEntityID=bc.PersonID
JOIN dbo.Password_Copy AS pw ON pw.BusinessEntityID=p.BusinessEntityID;
GO

SELECT TOP (20) * FROM dbo.vEntityContacts ORDER BY BusinessEntityID,PersonID;
SELECT TOP (20) * FROM dbo.vEntityContactDetails ORDER BY BusinessEntityID,PersonID;
GO
-- Изменяем оба многотабличных представления, добавляя столбец.
ALTER VIEW dbo.vEntityContacts AS
SELECT be.BusinessEntityID,bc.PersonID,p.Title,p.FirstName,p.LastName,p.PersonType
FROM dbo.BusinessEntity_Copy AS be
JOIN dbo.BusinessEntityContact_Copy AS bc
  ON bc.BusinessEntityID=be.BusinessEntityID
JOIN dbo.Person_Copy AS p ON p.BusinessEntityID=bc.PersonID;
GO
ALTER VIEW dbo.vEntityContactDetails AS
SELECT be.BusinessEntityID,ba.AddressID,bc.PersonID,
       p.Title,p.FirstName,p.LastName,p.PersonType,pw.ModifiedDate AS PasswordDate
FROM dbo.BusinessEntity_Copy AS be
JOIN dbo.BusinessEntityAddress_Copy AS ba
  ON ba.BusinessEntityID=be.BusinessEntityID
JOIN dbo.BusinessEntityContact_Copy AS bc
  ON bc.BusinessEntityID=be.BusinessEntityID
JOIN dbo.Person_Copy AS p ON p.BusinessEntityID=bc.PersonID
JOIN dbo.Password_Copy AS pw ON pw.BusinessEntityID=p.BusinessEntityID;
GO
SELECT TOP (10) * FROM dbo.vEntityContacts;
SELECT TOP (10) * FROM dbo.vEntityContactDetails;
GO

-- Сверить предыдущие COUNT: в стандартной базе самые большие из этих
-- пяти таблиц обычно BusinessEntity и Person. Для них создаём полные копии.
SELECT * INTO dbo.BusinessEntity_NoIndex FROM dbo.BusinessEntity_Copy;
SELECT * INTO dbo.Person_NoIndex FROM dbo.Person_Copy;
SELECT * INTO dbo.BusinessEntity_Indexed FROM dbo.BusinessEntity_Copy;
SELECT * INTO dbo.Person_Indexed FROM dbo.Person_Copy;
GO

-- В SSMS включить "Фактический план выполнения" (Ctrl+M).
-- Сравнить Logical reads, CPU time, elapsed time в Messages и операторы плана.
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
SELECT COUNT(*) FROM dbo.BusinessEntity_NoIndex;
SELECT COUNT(*) FROM dbo.Person_NoIndex;
SELECT * FROM dbo.BusinessEntity_NoIndex WHERE BusinessEntityID=100;
SELECT BusinessEntityID,FirstName,LastName
FROM dbo.Person_NoIndex WHERE LastName=N'Smith';
GO

-- Кластеризованные поля: BusinessEntityID в обеих индексированных копиях.
CREATE CLUSTERED INDEX CX_BE_Indexed_Id
ON dbo.BusinessEntity_Indexed(BusinessEntityID);
CREATE CLUSTERED INDEX CX_Person_Indexed_Id
ON dbo.Person_Indexed(BusinessEntityID);
GO
SELECT COUNT(*) FROM dbo.BusinessEntity_Indexed;
SELECT COUNT(*) FROM dbo.Person_Indexed;
SELECT * FROM dbo.BusinessEntity_Indexed WHERE BusinessEntityID=100;
SELECT BusinessEntityID,FirstName,LastName
FROM dbo.Person_Indexed WHERE LastName=N'Smith';
GO

-- Индексируем поле, которое не является ключом кластеризации.
CREATE NONCLUSTERED INDEX IX_Person_Indexed_LastName
ON dbo.Person_Indexed(LastName) INCLUDE (FirstName);
CREATE NONCLUSTERED INDEX IX_BE_Indexed_ModifiedDate
ON dbo.BusinessEntity_Indexed(ModifiedDate);
GO
SELECT BusinessEntityID,FirstName,LastName
FROM dbo.Person_Indexed WHERE LastName=N'Smith';
SELECT BusinessEntityID,ModifiedDate
FROM dbo.BusinessEntity_Indexed
WHERE ModifiedDate >= '2014-01-01';
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
GO
