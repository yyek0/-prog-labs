/* Лабораторная работа 9. Транзакции и триггеры.
   Скрипт выполняется после лабораторной работы 8 в БД Shop_Kashapov. */

USE Shop_Kashapov;
GO

CREATE TABLE dbo.OrderAudit
(
    AuditId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    OrderId INT NOT NULL,
    ActionName NVARCHAR(20) NOT NULL,
    OldStatus NVARCHAR(30) NULL,
    NewStatus NVARCHAR(30) NULL,
    ChangedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    ChangedBy NVARCHAR(128) NOT NULL DEFAULT SUSER_SNAME()
);
GO

CREATE OR ALTER TRIGGER dbo.tr_Orders_AfterInsert
ON dbo.Orders
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.OrderAudit (OrderId, ActionName, NewStatus)
    SELECT Id, N'INSERT', Status
    FROM inserted;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_Orders_AfterUpdate
ON dbo.Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.OrderAudit (OrderId, ActionName, OldStatus, NewStatus)
    SELECT i.Id, N'UPDATE', d.Status, i.Status
    FROM inserted AS i
    JOIN deleted AS d ON d.Id = i.Id
    WHERE ISNULL(i.Status, N'') <> ISNULL(d.Status, N'');
END;
GO

-- Транзакция INSERT: добавление заказа и уменьшение остатка товара.
BEGIN TRANSACTION;

INSERT INTO dbo.Orders (CustomerId, ProductId, Quantity, UnitPrice, Status)
SELECT 1, 10, 1, Price, N'Оплачен'
FROM dbo.Products
WHERE Id = 10;

UPDATE dbo.Products
SET ProductCount = ProductCount - 1
WHERE Id = 10 AND ProductCount > 0;

COMMIT TRANSACTION;
GO

-- Транзакция UPDATE: изменение статуса заказа.
BEGIN TRANSACTION;

UPDATE dbo.Orders
SET Status = N'Отправлен'
WHERE Id = 1;

COMMIT TRANSACTION;
GO

-- Транзакция DELETE с отменой: данные не удаляются после ROLLBACK.
BEGIN TRANSACTION;

DELETE FROM dbo.Orders
WHERE Id = 2;

ROLLBACK TRANSACTION;
GO

SELECT *
FROM dbo.Orders
ORDER BY Id;

SELECT *
FROM dbo.OrderAudit
ORDER BY AuditId;
GO
