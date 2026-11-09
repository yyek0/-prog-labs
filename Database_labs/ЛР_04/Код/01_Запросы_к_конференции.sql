/* Лабораторная работа 4. Запросы SELECT, WHERE, ORDER BY, GROUP BY.
   Дополнительные самостоятельные запросы к БД «Научная конференция». */

USE Conference_Kashapov;
GO

-- 1. Участники из Бирска.
SELECT LastName, FirstName, Organization, Email
FROM dbo.Participant
WHERE City = N'Бирск';

-- 2. Статьи объёмом не менее восьми страниц.
SELECT Title, PageCount, SubmittedAt
FROM dbo.Article
WHERE PageCount >= 8
ORDER BY PageCount DESC;

-- 3. Количество статей по секциям.
SELECT s.Name AS Секция, COUNT(a.ArticleId) AS Количество_статей
FROM dbo.Section AS s
LEFT JOIN dbo.Session AS se ON se.SectionId = s.SectionId
LEFT JOIN dbo.SessionReport AS sr ON sr.SessionId = se.SessionId
LEFT JOIN dbo.Article AS a ON a.ArticleId = sr.ArticleId
GROUP BY s.Name
ORDER BY Количество_статей DESC;

-- 4. Среднее число страниц в статьях конференции.
SELECT c.Name AS Конференция, AVG(CAST(a.PageCount AS DECIMAL(10, 2))) AS Среднее_число_страниц
FROM dbo.Conference AS c
JOIN dbo.Article AS a ON a.ConferenceId = c.ConferenceId
GROUP BY c.Name;

-- 5. Доклады, запланированные до обеденного перерыва.
SELECT sr.Speaker, a.Title, sr.StartTime, sr.EndTime
FROM dbo.SessionReport AS sr
JOIN dbo.Article AS a ON a.ArticleId = sr.ArticleId
WHERE sr.StartTime < '12:00'
ORDER BY sr.StartTime;

-- 6. Аудитории вместимостью от сорока человек.
SELECT Building, RoomNumber, Capacity, ResponsiblePerson
FROM dbo.Room
WHERE Capacity >= 40
ORDER BY Capacity DESC;

-- 7. Количество участников по форме участия.
SELECT ParticipationForm, COUNT(*) AS Количество_участников
FROM dbo.Participant
GROUP BY ParticipationForm;

-- 8. Авторы статей и названия их работ.
SELECT p.LastName, p.FirstName, a.Title, a.Status
FROM dbo.Participant AS p
JOIN dbo.Article AS a ON a.ParticipantId = p.ParticipantId
ORDER BY p.LastName;

-- 9. Секции и количество запланированных докладов.
SELECT Name, PlannedReports
FROM dbo.Section
WHERE PlannedReports > 0
ORDER BY PlannedReports DESC;

-- 10. Максимальная посещаемость по секциям.
SELECT s.Name AS Секция, MAX(sr.AttendanceCount) AS Максимальная_посещаемость
FROM dbo.Section AS s
JOIN dbo.Session AS se ON se.SectionId = s.SectionId
JOIN dbo.SessionReport AS sr ON sr.SessionId = se.SessionId
GROUP BY s.Name;
GO
