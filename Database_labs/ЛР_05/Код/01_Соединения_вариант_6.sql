/* ЛР 5, часть 4, вариант 6. AdventureWorks2019, схема Person.
   Таблицы: BusinessEntity, BusinessEntityAddress, BusinessEntityContact,
   Person, Password. Дополнительная таблица EmailAddress для множеств.
   Запросы запускать после восстановления AdventureWorks2019. */
USE AdventureWorks2019;
GO
-- Сначала смотрим, сколько строк в таблицах и как они связаны.
SELECT N'BusinessEntity' AS TableName, COUNT(*) AS RowsCount FROM Person.BusinessEntity
UNION ALL SELECT N'BusinessEntityAddress',COUNT(*) FROM Person.BusinessEntityAddress
UNION ALL SELECT N'BusinessEntityContact',COUNT(*) FROM Person.BusinessEntityContact
UNION ALL SELECT N'Person',COUNT(*) FROM Person.Person
UNION ALL SELECT N'Password',COUNT(*) FROM Person.Password;
GO

/* Ветка из двух таблиц. */

-- 1а. INNER JOIN без группировки: двух таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, ba.AddressTypeID
FROM Person.BusinessEntity AS be
INNER JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID);

-- 1б. INNER JOIN с группировкой: количество уникальных записей по группе.
SELECT CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END AS НаличиеАдреса, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
INNER JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
GROUP BY CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END
ORDER BY НаличиеАдреса;

-- 2а. LEFT JOIN без группировки: двух таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, ba.AddressTypeID
FROM Person.BusinessEntity AS be
LEFT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID);

-- 2б. LEFT JOIN с группировкой: количество уникальных записей по группе.
SELECT CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END AS НаличиеАдреса, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
LEFT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
GROUP BY CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END
ORDER BY НаличиеАдреса;

-- 3а. RIGHT JOIN без группировки: двух таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, ba.AddressTypeID
FROM Person.BusinessEntity AS be
RIGHT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID);

-- 3б. RIGHT JOIN с группировкой: количество уникальных записей по группе.
SELECT CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END AS НаличиеАдреса, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
RIGHT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
GROUP BY CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END
ORDER BY НаличиеАдреса;

-- 4а. FULL JOIN без группировки: двух таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, ba.AddressTypeID
FROM Person.BusinessEntity AS be
FULL JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID);

-- 4б. FULL JOIN с группировкой: количество уникальных записей по группе.
SELECT CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END AS НаличиеАдреса, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
FULL JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
GROUP BY CASE WHEN ba.AddressID IS NULL THEN N'Без адреса' ELSE N'Есть адрес' END
ORDER BY НаличиеАдреса;

/* Ветка из трёх таблиц. */

-- 5а. INNER JOIN без группировки: трёх таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, bc.PersonID AS ContactId, p.FirstName, p.LastName, p.PersonType
FROM Person.BusinessEntity AS be
INNER JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
INNER JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
ORDER BY COALESCE(be.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID);

-- 5б. INNER JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет персоны') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов
FROM Person.BusinessEntity AS be
INNER JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
INNER JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
GROUP BY COALESCE(p.PersonType, N'Нет персоны')
ORDER BY ТипКонтакта;

-- 6а. LEFT JOIN без группировки: трёх таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, bc.PersonID AS ContactId, p.FirstName, p.LastName, p.PersonType
FROM Person.BusinessEntity AS be
LEFT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
LEFT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
ORDER BY COALESCE(be.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID);

-- 6б. LEFT JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет персоны') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов
FROM Person.BusinessEntity AS be
LEFT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
LEFT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
GROUP BY COALESCE(p.PersonType, N'Нет персоны')
ORDER BY ТипКонтакта;

-- 7а. RIGHT JOIN без группировки: трёх таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, bc.PersonID AS ContactId, p.FirstName, p.LastName, p.PersonType
FROM Person.BusinessEntity AS be
RIGHT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
RIGHT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
ORDER BY COALESCE(be.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID);

-- 7б. RIGHT JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет персоны') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов
FROM Person.BusinessEntity AS be
RIGHT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
RIGHT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
GROUP BY COALESCE(p.PersonType, N'Нет персоны')
ORDER BY ТипКонтакта;

-- 8а. FULL JOIN без группировки: трёх таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, bc.PersonID AS ContactId, p.FirstName, p.LastName, p.PersonType
FROM Person.BusinessEntity AS be
FULL JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
FULL JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
ORDER BY COALESCE(be.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID);

-- 8б. FULL JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет персоны') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов
FROM Person.BusinessEntity AS be
FULL JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
FULL JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
GROUP BY COALESCE(p.PersonType, N'Нет персоны')
ORDER BY ТипКонтакта;

/* Ветка из пяти таблиц. */

-- 9а. INNER JOIN без группировки: пяти таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, bc.PersonID AS ContactId, p.FirstName, p.LastName, pw.ModifiedDate AS PasswordChangedAt
FROM Person.BusinessEntity AS be
INNER JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
INNER JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
INNER JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
INNER JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID, pw.BusinessEntityID);

-- 9б. INNER JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет контакта') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
INNER JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
INNER JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
INNER JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
INNER JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
GROUP BY COALESCE(p.PersonType, N'Нет контакта')
ORDER BY ТипКонтакта;

-- 10а. LEFT JOIN без группировки: пяти таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, bc.PersonID AS ContactId, p.FirstName, p.LastName, pw.ModifiedDate AS PasswordChangedAt
FROM Person.BusinessEntity AS be
LEFT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
LEFT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
LEFT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
LEFT JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID, pw.BusinessEntityID);

-- 10б. LEFT JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет контакта') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
LEFT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
LEFT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
LEFT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
LEFT JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
GROUP BY COALESCE(p.PersonType, N'Нет контакта')
ORDER BY ТипКонтакта;

-- 11а. RIGHT JOIN без группировки: пяти таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, bc.PersonID AS ContactId, p.FirstName, p.LastName, pw.ModifiedDate AS PasswordChangedAt
FROM Person.BusinessEntity AS be
RIGHT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
RIGHT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
RIGHT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
RIGHT JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID, pw.BusinessEntityID);

-- 11б. RIGHT JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет контакта') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
RIGHT JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
RIGHT JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
RIGHT JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
RIGHT JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
GROUP BY COALESCE(p.PersonType, N'Нет контакта')
ORDER BY ТипКонтакта;

-- 12а. FULL JOIN без группировки: пяти таблиц, первые 20 строк.
SELECT TOP (20) be.BusinessEntityID AS EntityId, ba.AddressID, bc.PersonID AS ContactId, p.FirstName, p.LastName, pw.ModifiedDate AS PasswordChangedAt
FROM Person.BusinessEntity AS be
FULL JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
FULL JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
FULL JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
FULL JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
ORDER BY COALESCE(be.BusinessEntityID, ba.BusinessEntityID, bc.BusinessEntityID, p.BusinessEntityID, pw.BusinessEntityID);

-- 12б. FULL JOIN с группировкой: количество уникальных записей по группе.
SELECT COALESCE(p.PersonType, N'Нет контакта') AS ТипКонтакта, COUNT(DISTINCT be.BusinessEntityID) AS Организаций, COUNT(DISTINCT bc.PersonID) AS Контактов, COUNT(DISTINCT ba.AddressID) AS Адресов
FROM Person.BusinessEntity AS be
FULL JOIN Person.BusinessEntityAddress AS ba ON ba.BusinessEntityID = be.BusinessEntityID
FULL JOIN Person.BusinessEntityContact AS bc ON bc.BusinessEntityID = be.BusinessEntityID
FULL JOIN Person.Person AS p ON p.BusinessEntityID = bc.PersonID
FULL JOIN Person.Password AS pw ON pw.BusinessEntityID = p.BusinessEntityID
GROUP BY COALESCE(p.PersonType, N'Нет контакта')
ORDER BY ТипКонтакта;

-- 13. CROSS JOIN: шесть сочетаний трёх сотрудников и двух типов адресов.
SELECT p.BusinessEntityID, p.LastName, t.AddressTypeID, t.Name AS AddressType
FROM (SELECT TOP (3) BusinessEntityID,LastName FROM Person.Person ORDER BY BusinessEntityID) AS p
CROSS JOIN (SELECT TOP (2) AddressTypeID,Name FROM Person.AddressType ORDER BY AddressTypeID) AS t;

-- 14. INTERSECT: контактные лица, у которых также есть email.
SELECT PersonID AS BusinessEntityID FROM Person.BusinessEntityContact
INTERSECT
SELECT BusinessEntityID FROM Person.EmailAddress;

-- 15. UNION: объединение без повторов.
SELECT PersonID AS BusinessEntityID FROM Person.BusinessEntityContact
UNION
SELECT BusinessEntityID FROM Person.EmailAddress;

-- 16. UNION ALL: объединение с повторами.
SELECT PersonID AS BusinessEntityID FROM Person.BusinessEntityContact
UNION ALL
SELECT BusinessEntityID FROM Person.EmailAddress;

-- 17. EXCEPT: контактные лица без записи в EmailAddress.
SELECT PersonID AS BusinessEntityID FROM Person.BusinessEntityContact
EXCEPT
SELECT BusinessEntityID FROM Person.EmailAddress;
GO
