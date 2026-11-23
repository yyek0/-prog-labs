/* ЛР 6, часть 2, вариант 6. Подзапросы вместо соединения Person
   и BusinessEntityAddress из ЛР 5. Запустить в AdventureWorks2019. */
USE AdventureWorks2019;
GO

-- 1. Несвязанный табличный подзапрос без группировки:
--    персоны, для которых в таблице адресов есть запись.
SELECT p.BusinessEntityID, p.FirstName, p.LastName
FROM Person.Person AS p
WHERE p.BusinessEntityID IN
    (SELECT a.BusinessEntityID FROM Person.BusinessEntityAddress AS a)
ORDER BY p.LastName, p.FirstName;

-- 2. Тот же несвязанный подзапрос с группировкой: число персон по типу.
SELECT p.PersonType, COUNT(*) AS PeopleWithAddress
FROM Person.Person AS p
WHERE p.BusinessEntityID IN
    (SELECT a.BusinessEntityID FROM Person.BusinessEntityAddress AS a)
GROUP BY p.PersonType
ORDER BY PeopleWithAddress DESC;

-- 3. Связанный табличный подзапрос EXISTS без группировки:
--    юридические лица, у которых зарегистрирован хотя бы один адрес.
SELECT be.BusinessEntityID, be.ModifiedDate
FROM Person.BusinessEntity AS be
WHERE EXISTS
    (SELECT 1 FROM Person.BusinessEntityAddress AS a
     WHERE a.BusinessEntityID = be.BusinessEntityID)
ORDER BY be.BusinessEntityID;

-- 4. Связанный подзапрос с группировкой: количество персон без адреса.
SELECT p.PersonType, COUNT(*) AS PeopleWithoutAddress
FROM Person.Person AS p
WHERE NOT EXISTS
    (SELECT 1 FROM Person.BusinessEntityAddress AS a
     WHERE a.BusinessEntityID = p.BusinessEntityID)
GROUP BY p.PersonType
ORDER BY PeopleWithoutAddress DESC;

-- 5. Связанный скалярный подзапрос: число адресов у каждой персоны.
SELECT TOP (30) p.BusinessEntityID, p.FirstName, p.LastName,
       (SELECT COUNT(*) FROM Person.BusinessEntityAddress AS a
        WHERE a.BusinessEntityID = p.BusinessEntityID) AS AddressCount
FROM Person.Person AS p
ORDER BY AddressCount DESC, p.BusinessEntityID;

-- 6. Скалярный подзапрос плюс группировка: сумма адресов по типам персон.
SELECT counts.PersonType, COUNT(*) AS PeopleCount,
       SUM(counts.AddressCount) AS AddressCount
FROM
(
    SELECT p.PersonType,
           (SELECT COUNT(*) FROM Person.BusinessEntityAddress AS a
            WHERE a.BusinessEntityID = p.BusinessEntityID) AS AddressCount
    FROM Person.Person AS p
) AS counts
GROUP BY counts.PersonType
ORDER BY AddressCount DESC;

-- 7. Несвязанный скалярный подзапрос без группировки:
--    записи адресов, изменённые в день самого позднего изменения.
SELECT a.BusinessEntityID, a.AddressID, a.ModifiedDate
FROM Person.BusinessEntityAddress AS a
WHERE CONVERT(date,a.ModifiedDate) =
    (SELECT CONVERT(date,MAX(ModifiedDate))
     FROM Person.BusinessEntityAddress);

-- 8. Несвязанный скалярный подзапрос с группировкой:
--    типы персон, среднее число адресов которых выше общего среднего.
SELECT x.PersonType, AVG(CAST(x.AddressCount AS decimal(12,2))) AS AverageAddresses
FROM
(
    SELECT p.PersonType,
           (SELECT COUNT(*) FROM Person.BusinessEntityAddress AS a
            WHERE a.BusinessEntityID = p.BusinessEntityID) AS AddressCount
    FROM Person.Person AS p
) AS x
GROUP BY x.PersonType
HAVING AVG(CAST(x.AddressCount AS decimal(12,2))) >
(
    SELECT AVG(CAST(y.AddressCount AS decimal(12,2)))
    FROM
    (
        SELECT
            (SELECT COUNT(*) FROM Person.BusinessEntityAddress AS a2
             WHERE a2.BusinessEntityID = p2.BusinessEntityID) AS AddressCount
        FROM Person.Person AS p2
    ) AS y
);
GO
