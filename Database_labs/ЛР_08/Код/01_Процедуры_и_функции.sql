/* ЛР 8. Процедуры и пользовательские функции для варианта 6.
   Работаем с базой повышения квалификации из ЛР 2–3. */
USE UniversityTraining_V6;
GO

-- 1. Процедура без выходных параметров: список курсов по статусу.
CREATE OR ALTER PROCEDURE dbo.ListCourses
    @Status NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CourseId,c.CourseName,c.StartDate,c.EndDate,c.Hours,
           e.LastName+N' '+e.FirstName AS Teacher,
           c.MaxParticipants,c.Status
    FROM dbo.Course AS c
    JOIN dbo.Employee AS e ON e.EmployeeId=c.TeacherId
    WHERE @Status IS NULL OR c.Status=@Status
    ORDER BY c.StartDate,c.CourseName;
END;
GO

-- 2. Скалярная функция: сколько свободных мест осталось на курсе.
CREATE OR ALTER FUNCTION dbo.FreePlaces(@CourseId INT)
RETURNS INT
AS
BEGIN
    DECLARE @Free INT;
    SELECT @Free=c.MaxParticipants-
        (SELECT COUNT(*) FROM dbo.Enrollment AS en
         WHERE en.CourseId=c.CourseId AND en.Result<>N'Отменён')
    FROM dbo.Course AS c WHERE c.CourseId=@CourseId;
    RETURN @Free;
END;
GO

-- 3. Табличная функция: все курсы конкретного сотрудника.
CREATE OR ALTER FUNCTION dbo.EmployeeCourses(@EmployeeId INT)
RETURNS TABLE
AS
RETURN
(
    SELECT c.CourseName,c.StartDate,c.EndDate,en.Result,en.FinalScore
    FROM dbo.Enrollment AS en
    JOIN dbo.Course AS c ON c.CourseId=en.CourseId
    WHERE en.EmployeeId=@EmployeeId
);
GO

-- 4. Процедура с входными и выходным параметром: запись сотрудника.
CREATE OR ALTER PROCEDURE dbo.RegisterEmployee
    @EmployeeId INT,
    @CourseId INT,
    @EnrollmentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @Capacity INT, @Status NVARCHAR(30);
        SELECT @Capacity=MaxParticipants,@Status=Status
        FROM dbo.Course WITH (UPDLOCK,HOLDLOCK)
        WHERE CourseId=@CourseId;
        IF @Capacity IS NULL THROW 50001,N'Курс не найден',1;
        IF @Status<>N'Открыт' THROW 50002,N'Запись на курс закрыта',1;
        IF NOT EXISTS (SELECT 1 FROM dbo.Employee WHERE EmployeeId=@EmployeeId AND IsActive=1)
            THROW 50003,N'Сотрудник не найден или не работает',1;
        IF EXISTS (SELECT 1 FROM dbo.Enrollment WHERE EmployeeId=@EmployeeId AND CourseId=@CourseId)
            THROW 50004,N'Сотрудник уже записан',1;
        IF (SELECT COUNT(*) FROM dbo.Enrollment
            WHERE CourseId=@CourseId AND Result<>N'Отменён')>=@Capacity
            THROW 50005,N'На курсе нет свободных мест',1;

        SELECT @EnrollmentId=ISNULL(MAX(EnrollmentId),0)+1
        FROM dbo.Enrollment WITH (UPDLOCK,HOLDLOCK);
        INSERT dbo.Enrollment(EnrollmentId,CourseId,EmployeeId,EnrolledAt,
                              Result,PaymentSource,IsCompleted)
        VALUES(@EnrollmentId,@CourseId,@EmployeeId,CONVERT(date,GETDATE()),
               N'Записан',N'Университет',0);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- 5. Завершение обучения и выдача свидетельства в одной транзакции.
CREATE OR ALTER PROCEDURE dbo.IssueCertificate
    @EnrollmentId INT,
    @Score DECIMAL(5,2),
    @CertificateNumber NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF @Score<60 OR @Score>100
            THROW 50006,N'Для свидетельства нужен результат от 60 до 100',1;
        IF EXISTS (SELECT 1 FROM dbo.Certificate WHERE EnrollmentId=@EnrollmentId)
            THROW 50007,N'Свидетельство уже выдано',1;
        IF NOT EXISTS
        (
            SELECT 1 FROM dbo.Enrollment AS en
            JOIN dbo.Course AS c ON c.CourseId=en.CourseId
            WHERE en.EnrollmentId=@EnrollmentId AND c.EndDate<=CONVERT(date,GETDATE())
        ) THROW 50008,N'Обучение ещё не завершилось',1;

        UPDATE dbo.Enrollment
        SET FinalScore=@Score,AttendancePercent=100,
            Result=N'Сдано',IsCompleted=1
        WHERE EnrollmentId=@EnrollmentId;

        DECLARE @CertificateId INT;
        SELECT @CertificateId=ISNULL(MAX(CertificateId),0)+1
        FROM dbo.Certificate WITH (UPDLOCK,HOLDLOCK);
        INSERT dbo.Certificate(CertificateId,EnrollmentId,CertificateNumber,
                               IssueDate,Qualification,Hours,IssuedBy,IsValid)
        SELECT @CertificateId,en.EnrollmentId,@CertificateNumber,
               CONVERT(date,GETDATE()),c.CourseName,c.Hours,N'Учебный отдел',1
        FROM dbo.Enrollment AS en
        JOIN dbo.Course AS c ON c.CourseId=en.CourseId
        WHERE en.EnrollmentId=@EnrollmentId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Контрольные вызовы без постоянного изменения исходных данных.
EXEC dbo.ListCourses @Status=N'Открыт';
SELECT c.CourseId,c.CourseName,dbo.FreePlaces(c.CourseId) AS FreePlaces
FROM dbo.Course AS c;
SELECT * FROM dbo.EmployeeCourses(2);
GO
BEGIN TRANSACTION;
DECLARE @NewId INT;
EXEC dbo.RegisterEmployee @EmployeeId=6,@CourseId=4,@EnrollmentId=@NewId OUTPUT;
SELECT @NewId AS NewEnrollmentId;
SELECT * FROM dbo.Enrollment WHERE EnrollmentId=@NewId;
ROLLBACK TRANSACTION;
GO
