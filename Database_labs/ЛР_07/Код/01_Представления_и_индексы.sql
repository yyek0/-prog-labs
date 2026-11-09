/* Лабораторная работа 7. Представления и индексы. Вариант 7. */

IF DB_ID(N'AdventureWorks_Kashapov') IS NOT NULL
BEGIN
    ALTER DATABASE AdventureWorks_Kashapov SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AdventureWorks_Kashapov;
END;
GO

CREATE DATABASE AdventureWorks_Kashapov;
GO

USE AdventureWorks_Kashapov;
GO

-- Копии таблиц варианта из учебной БД.
SELECT * INTO dbo.EmailAddress_Copy FROM AdventureWorks2019.Person.EmailAddress;
SELECT BusinessEntityID, PersonType, NameStyle, Title, FirstName, MiddleName, LastName, Suffix,
       EmailPromotion, CONVERT(XML, AdditionalContactInfo) AS AdditionalContactInfo,
       CONVERT(XML, Demographics) AS Demographics, rowguid, ModifiedDate
INTO dbo.Person_Copy
FROM AdventureWorks2019.Person.Person;
SELECT * INTO dbo.BusinessEntity_Copy FROM AdventureWorks2019.Person.BusinessEntity;
SELECT * INTO dbo.Password_Copy FROM AdventureWorks2019.Person.Password;
SELECT * INTO dbo.BusinessEntityContact_Copy FROM AdventureWorks2019.Person.BusinessEntityContact;
GO

CREATE VIEW dbo.vw_EmailAddress AS
SELECT BusinessEntityID, EmailAddressID, EmailAddress, ModifiedDate
FROM dbo.EmailAddress_Copy;
GO

CREATE VIEW dbo.vw_Person AS
SELECT BusinessEntityID, PersonType, FirstName, MiddleName, LastName, ModifiedDate
FROM dbo.Person_Copy;
GO

CREATE VIEW dbo.vw_BusinessEntity AS
SELECT BusinessEntityID, ModifiedDate
FROM dbo.BusinessEntity_Copy;
GO

CREATE VIEW dbo.vw_Password AS
SELECT BusinessEntityID, ModifiedDate
FROM dbo.Password_Copy;
GO

CREATE VIEW dbo.vw_BusinessEntityContact AS
SELECT BusinessEntityID, PersonID, ContactTypeID, ModifiedDate
FROM dbo.BusinessEntityContact_Copy;
GO

-- Представление из трёх таблиц.
CREATE VIEW dbo.vw_PersonEmailEntity AS
SELECT p.BusinessEntityID, p.LastName, p.FirstName, ea.EmailAddress, be.ModifiedDate
FROM dbo.Person_Copy AS p
JOIN dbo.EmailAddress_Copy AS ea ON ea.BusinessEntityID = p.BusinessEntityID
JOIN dbo.BusinessEntity_Copy AS be ON be.BusinessEntityID = p.BusinessEntityID;
GO

-- Представление из пяти таблиц.
CREATE VIEW dbo.vw_FullContactInfo AS
SELECT p.BusinessEntityID, p.LastName, p.FirstName, ea.EmailAddress,
       pa.ModifiedDate AS PasswordModified, bec.ContactTypeID
FROM dbo.Person_Copy AS p
LEFT JOIN dbo.EmailAddress_Copy AS ea ON ea.BusinessEntityID = p.BusinessEntityID
LEFT JOIN dbo.Password_Copy AS pa ON pa.BusinessEntityID = p.BusinessEntityID
LEFT JOIN dbo.BusinessEntityContact_Copy AS bec ON bec.BusinessEntityID = p.BusinessEntityID;
GO

SELECT TOP (20) * FROM dbo.vw_PersonEmailEntity ORDER BY LastName;
SELECT TOP (20) * FROM dbo.vw_FullContactInfo ORDER BY LastName;
GO

-- Изменение представления: к полям добавлен тип персоны.
ALTER VIEW dbo.vw_PersonEmailEntity AS
SELECT p.BusinessEntityID, p.PersonType, p.LastName, p.FirstName, ea.EmailAddress, be.ModifiedDate
FROM dbo.Person_Copy AS p
JOIN dbo.EmailAddress_Copy AS ea ON ea.BusinessEntityID = p.BusinessEntityID
JOIN dbo.BusinessEntity_Copy AS be ON be.BusinessEntityID = p.BusinessEntityID;
GO

-- Полные дубликаты двух наиболее объёмных таблиц.
SELECT * INTO dbo.Person_Test_NoIndex FROM dbo.Person_Copy;
SELECT * INTO dbo.EmailAddress_Test_NoIndex FROM dbo.EmailAddress_Copy;
SELECT * INTO dbo.Person_Test_Indexed FROM dbo.Person_Copy;
SELECT * INTO dbo.EmailAddress_Test_Indexed FROM dbo.EmailAddress_Copy;
GO

CREATE CLUSTERED INDEX CX_Person_Test_Indexed_BusinessEntityID
ON dbo.Person_Test_Indexed (BusinessEntityID);

CREATE CLUSTERED INDEX CX_EmailAddress_Test_Indexed_EmailAddressID
ON dbo.EmailAddress_Test_Indexed (EmailAddressID);

CREATE NONCLUSTERED INDEX IX_Person_Test_Indexed_LastName
ON dbo.Person_Test_Indexed (LastName, FirstName);

CREATE NONCLUSTERED INDEX IX_EmailAddress_Test_Indexed_BusinessEntityID
ON dbo.EmailAddress_Test_Indexed (BusinessEntityID);
GO

-- Сравнение запросов: план выполнения анализируется в SSMS до и после создания индексов.
SELECT * FROM dbo.Person_Test_NoIndex WHERE LastName = N'Smith';
SELECT * FROM dbo.Person_Test_Indexed WHERE LastName = N'Smith';
SELECT * FROM dbo.EmailAddress_Test_NoIndex WHERE BusinessEntityID = 1;
SELECT * FROM dbo.EmailAddress_Test_Indexed WHERE BusinessEntityID = 1;
GO

