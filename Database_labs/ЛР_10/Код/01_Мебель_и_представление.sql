/* Лабораторная работа 10. БД «Мебель» и представление ListFurniture. */

IF DB_ID(N'Furniture_Kashapov') IS NULL
BEGIN
    CREATE DATABASE Furniture_Kashapov;
END;
GO

USE Furniture_Kashapov;
GO

CREATE TABLE dbo.Category
(
    CategoryId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    NameCategory NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(300) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE dbo.Furniture
(
    FurnitureId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    CategoryId INT NOT NULL,
    NameFurniture NVARCHAR(150) NOT NULL,
    Manufacturer NVARCHAR(100) NOT NULL,
    Price MONEY NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    Material NVARCHAR(80) NOT NULL,
    Color NVARCHAR(50) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Furniture_Category FOREIGN KEY (CategoryId)
        REFERENCES dbo.Category (CategoryId),
    CONSTRAINT CK_Furniture_Price CHECK (Price > 0),
    CONSTRAINT CK_Furniture_Quantity CHECK (Quantity >= 0)
);
GO

INSERT INTO dbo.Category (NameCategory, Description)
VALUES
    (N'Столы', N'Письменные и обеденные столы'),
    (N'Стулья', N'Стулья для дома и офиса'),
    (N'Шкафы', N'Шкафы для хранения вещей'),
    (N'Кровати', N'Односпальные и двуспальные кровати');

INSERT INTO dbo.Furniture
    (CategoryId, NameFurniture, Manufacturer, Price, Quantity, Material, Color)
VALUES
    (1, N'Стол письменный Лайт', N'МебельГрад', 12500, 8, N'ЛДСП', N'Белый'),
    (1, N'Стол обеденный Вега', N'Дом мебели', 18900, 5, N'Массив дерева', N'Орех'),
    (2, N'Стул Комфорт', N'МебельГрад', 5400, 18, N'Металл и ткань', N'Серый'),
    (2, N'Стул Офисный', N'ОфисСтиль', 7900, 11, N'Металл и экокожа', N'Черный'),
    (3, N'Шкаф Рио', N'Дом мебели', 24500, 4, N'ЛДСП', N'Дуб'),
    (3, N'Шкаф-купе Элит', N'МебельГрад', 38900, 3, N'ЛДСП и зеркало', N'Белый'),
    (4, N'Кровать Сканди', N'Соната', 27900, 6, N'Массив дерева', N'Натуральный');
GO

CREATE OR ALTER VIEW dbo.ListFurniture
AS
SELECT f.FurnitureId AS Id,
       f.NameFurniture AS FurnitureName,
       c.NameCategory AS CategoryName,
       f.Manufacturer,
       f.Price,
       f.Quantity,
       f.Material,
       f.Color,
       f.IsAvailable
FROM dbo.Furniture AS f
JOIN dbo.Category AS c ON c.CategoryId = f.CategoryId;
GO

SELECT *
FROM dbo.ListFurniture
ORDER BY CategoryName, FurnitureName;
GO
