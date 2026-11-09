/* Лабораторная работа 5. Соединения таблиц AdventureWorks2019. Вариант 7.
   Таблицы: Person.EmailAddress, Person.Person, Person.BusinessEntity,
   Person.Password, Person.BusinessEntityContact. */

USE AdventureWorks2019;
GO

-- INNER JOIN 1 без группировки: адреса электронной почты пользователей.
SELECT p.BusinessEntityID, p.LastName, p.FirstName, ea.EmailAddress
FROM Person.Person AS p
INNER JOIN Person.EmailAddress AS ea ON ea.BusinessEntityID = p.BusinessEntityID
ORDER BY p.LastName, p.FirstName;

-- INNER JOIN 2 с группировкой: число адресов электронной почты по типу персоны.
SELECT p.PersonType, COUNT(ea.EmailAddressID) AS Количество_адресов
FROM Person.Person AS p
INNER JOIN Person.EmailAddress AS ea ON ea.BusinessEntityID = p.BusinessEntityID
GROUP BY p.PersonType
ORDER BY Количество_адресов DESC;

-- LEFT JOIN 1 без группировки: персоны, для которых адрес электронной почты может отсутствовать.
SELECT p.BusinessEntityID, p.LastName, p.FirstName, ea.EmailAddress
FROM Person.Person AS p
LEFT JOIN Person.EmailAddress AS ea ON ea.BusinessEntityID = p.BusinessEntityID
ORDER BY p.BusinessEntityID;

-- LEFT JOIN 2 с группировкой: количество контактных лиц у каждой организации.
SELECT be.BusinessEntityID, COUNT(bec.PersonID) AS Количество_контактов
FROM Person.BusinessEntity AS be
LEFT JOIN Person.BusinessEntityContact AS bec ON bec.BusinessEntityID = be.BusinessEntityID
GROUP BY be.BusinessEntityID
ORDER BY Количество_контактов DESC;

-- RIGHT JOIN 1 без группировки: все адреса и их владельцы.
SELECT ea.EmailAddressID, ea.EmailAddress, p.LastName, p.FirstName
FROM Person.Person AS p
RIGHT JOIN Person.EmailAddress AS ea ON ea.BusinessEntityID = p.BusinessEntityID
ORDER BY ea.EmailAddressID;

-- RIGHT JOIN 2 с группировкой: адреса по типу персоны.
SELECT p.PersonType, COUNT(ea.EmailAddressID) AS Количество_адресов
FROM Person.Person AS p
RIGHT JOIN Person.EmailAddress AS ea ON ea.BusinessEntityID = p.BusinessEntityID
GROUP BY p.PersonType
ORDER BY Количество_адресов DESC;

-- FULL JOIN 1 без группировки: проверка совпадения данных о персонах и паролях.
SELECT p.BusinessEntityID AS Персона, p.LastName, pa.BusinessEntityID AS Пароль_для_персоны
FROM Person.Person AS p
FULL JOIN Person.Password AS pa ON pa.BusinessEntityID = p.BusinessEntityID
ORDER BY COALESCE(p.BusinessEntityID, pa.BusinessEntityID);

-- FULL JOIN 2 с группировкой: количество записей паролей по типу пользователя.
SELECT p.PersonType, COUNT(pa.BusinessEntityID) AS Количество_паролей
FROM Person.Person AS p
FULL JOIN Person.Password AS pa ON pa.BusinessEntityID = p.BusinessEntityID
GROUP BY p.PersonType;

-- Запрос из трёх связанных таблиц: персоны, адреса и записи организаций.
SELECT p.BusinessEntityID, p.LastName, p.FirstName, ea.EmailAddress, be.ModifiedDate
FROM Person.Person AS p
INNER JOIN Person.EmailAddress AS ea ON ea.BusinessEntityID = p.BusinessEntityID
INNER JOIN Person.BusinessEntity AS be ON be.BusinessEntityID = p.BusinessEntityID
ORDER BY p.LastName;

-- Запрос по ветке из пяти таблиц: персона, email, организация, пароль и контакт.
SELECT p.BusinessEntityID, p.LastName, p.FirstName, ea.EmailAddress,
       CASE WHEN pa.BusinessEntityID IS NULL THEN N'Нет пароля' ELSE N'Пароль задан' END AS Пароль,
       bec.ContactTypeID
FROM Person.Person AS p
LEFT JOIN Person.EmailAddress AS ea ON ea.BusinessEntityID = p.BusinessEntityID
INNER JOIN Person.BusinessEntity AS be ON be.BusinessEntityID = p.BusinessEntityID
LEFT JOIN Person.Password AS pa ON pa.BusinessEntityID = be.BusinessEntityID
LEFT JOIN Person.BusinessEntityContact AS bec ON bec.BusinessEntityID = be.BusinessEntityID
ORDER BY p.LastName, p.FirstName;

-- CROSS JOIN: все сочетания двух первых персон и первых двух типов контакта.
SELECT p.FirstName, p.LastName, ct.Name AS Тип_контакта
FROM (SELECT TOP (2) BusinessEntityID, FirstName, LastName FROM Person.Person ORDER BY BusinessEntityID) AS p
CROSS JOIN (SELECT TOP (2) ContactTypeID, Name FROM Person.ContactType ORDER BY ContactTypeID) AS ct;

-- Добавочная таблица Person.PhoneNumberType. Операторы множеств.
SELECT BusinessEntityID FROM Person.EmailAddress
INTERSECT
SELECT BusinessEntityID FROM Person.Password;

SELECT BusinessEntityID FROM Person.EmailAddress
UNION
SELECT BusinessEntityID FROM Person.Password;

SELECT BusinessEntityID FROM Person.EmailAddress
UNION ALL
SELECT BusinessEntityID FROM Person.Password;

SELECT BusinessEntityID FROM Person.EmailAddress
EXCEPT
SELECT BusinessEntityID FROM Person.Password;
GO
