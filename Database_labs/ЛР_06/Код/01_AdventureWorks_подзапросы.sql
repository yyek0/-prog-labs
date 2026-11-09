/* Лабораторная работа 6. Подзапросы на основе варианта 7 лабораторной работы 5. */

USE AdventureWorks2019;
GO

-- Некоррелированный табличный подзапрос: персоны с адресом электронной почты.
SELECT BusinessEntityID, FirstName, LastName
FROM Person.Person
WHERE BusinessEntityID IN
(
    SELECT BusinessEntityID
    FROM Person.EmailAddress
)
ORDER BY LastName, FirstName;

-- Некоррелированный табличный подзапрос: персоны без записи о пароле.
SELECT BusinessEntityID, FirstName, LastName
FROM Person.Person
WHERE BusinessEntityID NOT IN
(
    SELECT BusinessEntityID
    FROM Person.Password
);

-- Скалярный подзапрос: число email-адресов для каждой персоны.
SELECT p.BusinessEntityID, p.LastName, p.FirstName,
       (SELECT COUNT(*)
        FROM Person.EmailAddress AS ea
        WHERE ea.BusinessEntityID = p.BusinessEntityID) AS Количество_email
FROM Person.Person AS p
ORDER BY Количество_email DESC, p.LastName;

-- Коррелированный подзапрос: персоны с более чем одним адресом электронной почты.
SELECT p.BusinessEntityID, p.LastName, p.FirstName
FROM Person.Person AS p
WHERE 1 <
(
    SELECT COUNT(*)
    FROM Person.EmailAddress AS ea
    WHERE ea.BusinessEntityID = p.BusinessEntityID
);

-- EXISTS: организации, имеющие хотя бы одно контактное лицо.
SELECT be.BusinessEntityID, be.ModifiedDate
FROM Person.BusinessEntity AS be
WHERE EXISTS
(
    SELECT 1
    FROM Person.BusinessEntityContact AS bec
    WHERE bec.BusinessEntityID = be.BusinessEntityID
);

-- ALL: записи Person, идентификатор которых больше всех идентификаторов первых трёх паролей.
SELECT TOP (20) BusinessEntityID, FirstName, LastName
FROM Person.Person
WHERE BusinessEntityID > ALL
(
    SELECT TOP (3) BusinessEntityID
    FROM Person.Password
    ORDER BY BusinessEntityID
)
ORDER BY BusinessEntityID;

-- ANY: персоны, чьи идентификаторы совпадают с идентификатором хотя бы одного email.
SELECT BusinessEntityID, FirstName, LastName
FROM Person.Person
WHERE BusinessEntityID = ANY
(
    SELECT BusinessEntityID
    FROM Person.EmailAddress
);

-- Подзапрос с группировкой: типы персон, где среднее число email выше общего среднего.
SELECT p.PersonType, COUNT(*) AS Количество_персон
FROM Person.Person AS p
WHERE p.BusinessEntityID IN
(
    SELECT ea.BusinessEntityID
    FROM Person.EmailAddress AS ea
    GROUP BY ea.BusinessEntityID
    HAVING COUNT(*) >=
    (
        SELECT AVG(CAST(EmailCount AS DECIMAL(10, 2)))
        FROM
        (
            SELECT COUNT(*) AS EmailCount
            FROM Person.EmailAddress
            GROUP BY BusinessEntityID
        ) AS Counts
    )
)
GROUP BY p.PersonType;
GO
