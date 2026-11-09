/* Лабораторная работа 8. Хранимые процедуры. */

IF DB_ID(N'Shop_Kashapov') IS NULL
BEGIN
    CREATE DATABASE Shop_Kashapov;
END;
GO

USE Shop_Kashapov;
GO

CREATE TABLE dbo.Products
(
    Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ProductName NVARCHAR(80) NOT NULL,
    Manufacturer NVARCHAR(80) NOT NULL,
    ProductCount INT NOT NULL DEFAULT 0,
    Price MONEY NOT NULL,
    Category NVARCHAR(60) NOT NULL,
    Article NVARCHAR(30) NOT NULL UNIQUE,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT CK_Products_Count CHECK (ProductCount >= 0),
    CONSTRAINT CK_Products_Price CHECK (Price > 0)
);

CREATE TABLE dbo.Customers
(
    Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(120) NOT NULL UNIQUE,
    Phone NVARCHAR(30) NULL,
    City NVARCHAR(60) NOT NULL,
    RegisteredAt DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE)
);

CREATE TABLE dbo.Orders
(
    Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    CustomerId INT NOT NULL,
    ProductId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Quantity INT NOT NULL,
    UnitPrice MONEY NOT NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Новый',
    Comment NVARCHAR(200) NULL,
    CONSTRAINT FK_Orders_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Customers (Id),
    CONSTRAINT FK_Orders_Product FOREIGN KEY (ProductId) REFERENCES dbo.Products (Id),
    CONSTRAINT CK_Orders_Quantity CHECK (Quantity > 0)
);

CREATE TABLE dbo.Employees
(
    Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(30) NOT NULL,
    SurName NVARCHAR(30) NOT NULL,
    Dolgnost NVARCHAR(30) NULL
);
GO

INSERT INTO dbo.Products (ProductName, Manufacturer, ProductCount, Price, Category, Article)
VALUES
    (N'Ноутбук Aspire 5', N'Acer', 12, 59990, N'Ноутбуки', N'NB-001'),
    (N'Монитор 24P', N'Philips', 15, 15990, N'Мониторы', N'MN-002'),
    (N'Клавиатура K120', N'Logitech', 30, 2190, N'Периферия', N'KB-003'),
    (N'Мышь M185', N'Logitech', 25, 1490, N'Периферия', N'MS-004'),
    (N'Принтер L3250', N'Epson', 8, 22490, N'Принтеры', N'PR-005'),
    (N'Флеш-накопитель 64GB', N'Kingston', 40, 890, N'Накопители', N'FL-006'),
    (N'Веб-камера C270', N'Logitech', 18, 3490, N'Периферия', N'WC-007'),
    (N'Колонки Z120', N'Logitech', 20, 2790, N'Аудио', N'SP-008'),
    (N'Роутер AX12', N'TP-Link', 10, 4590, N'Сеть', N'RT-009'),
    (N'Наушники WH-CH520', N'Sony', 16, 5490, N'Аудио', N'HP-010');

INSERT INTO dbo.Customers (FirstName, LastName, Email, Phone, City)
VALUES
    (N'Роман', N'Кашапов', N'roman.kashapov@example.com', N'+7 900 111-11-11', N'Бирск'),
    (N'Анна', N'Васильева', N'anna.vasilyeva@example.com', N'+7 900 222-22-22', N'Уфа'),
    (N'Денис', N'Морозов', N'denis.morozov@example.com', N'+7 900 333-33-33', N'Бирск'),
    (N'Ирина', N'Соколова', N'irina.sokolova@example.com', N'+7 900 444-44-44', N'Казань'),
    (N'Олег', N'Петров', N'oleg.petrov@example.com', N'+7 900 555-55-55', N'Уфа');

INSERT INTO dbo.Orders (CustomerId, ProductId, Quantity, UnitPrice, Status)
VALUES
    (1, 1, 1, 59990, N'Оплачен'),
    (2, 2, 1, 15990, N'Оплачен'),
    (3, 3, 2, 2190, N'Новый'),
    (4, 4, 1, 1490, N'Отправлен'),
    (5, 5, 1, 22490, N'Оплачен'),
    (1, 6, 3, 890, N'Новый'),
    (2, 7, 1, 3490, N'Отправлен'),
    (3, 8, 2, 2790, N'Оплачен');

INSERT INTO dbo.Employees (FirstName, SurName, Dolgnost)
VALUES
    (N'Марина', N'Иванова', N'Директор'),
    (N'Сергей', N'Петров', N'Продавец'),
    (N'Елена', N'Смирнова', N'Клерк'),
    (N'Андрей', N'Кузнецов', N'Мастер');
GO

CREATE OR ALTER PROCEDURE dbo.ProductSummary
AS
BEGIN
    SELECT ProductName, Manufacturer, ProductCount, Price, Category
    FROM dbo.Products
    ORDER BY ProductName;
END;
GO

CREATE OR ALTER PROCEDURE dbo.AddProduct
    @Name NVARCHAR(80),
    @Manufacturer NVARCHAR(80),
    @Count INT = 1,
    @Price MONEY,
    @Category NVARCHAR(60),
    @Article NVARCHAR(30)
AS
BEGIN
    INSERT INTO dbo.Products (ProductName, Manufacturer, ProductCount, Price, Category, Article)
    VALUES (@Name, @Manufacturer, @Count, @Price, @Category, @Article);
END;
GO

CREATE OR ALTER PROCEDURE dbo.GetProductCount
    @Category NVARCHAR(60),
    @Count INT OUTPUT
AS
BEGIN
    SELECT @Count = COUNT(*)
    FROM dbo.Products
    WHERE Category = @Category;
END;
GO

CREATE OR ALTER PROCEDURE dbo.CreateOrder
    @CustomerId INT,
    @ProductId INT,
    @Quantity INT,
    @OrderId INT OUTPUT
AS
BEGIN
    DECLARE @Price MONEY;

    SELECT @Price = Price
    FROM dbo.Products
    WHERE Id = @ProductId;

    INSERT INTO dbo.Orders (CustomerId, ProductId, Quantity, UnitPrice)
    VALUES (@CustomerId, @ProductId, @Quantity, @Price);

    SET @OrderId = SCOPE_IDENTITY();
END;
GO

EXEC dbo.ProductSummary;
EXEC dbo.AddProduct N'Кабель HDMI', N'Baseus', 20, 690, N'Кабели', N'CB-011';

DECLARE @PeripheralCount INT;
EXEC dbo.GetProductCount N'Периферия', @PeripheralCount OUTPUT;
SELECT @PeripheralCount AS Количество_товаров_категории;

DECLARE @NewOrderId INT;
EXEC dbo.CreateOrder 5, 9, 1, @NewOrderId OUTPUT;
SELECT @NewOrderId AS Номер_созданного_заказа;
GO
