CREATE PROCEDURE [dbo].[sp_Statistic_Create]
	@Id UNIQUEIDENTIFIER,
    @CurrentValue DECIMAL(32, 16),
    @CreatedAt DATETIME
AS
    SET NOCOUNT ON;

    DECLARE @LastCurrentValue DECIMAL(32, 16);
    
    -- Отримати останнє значення CurrentValue з урахуванням CreatedAt
    SELECT TOP 1 @LastCurrentValue = [CurrentValue]
    FROM [dbo].[Statistic]
    WHERE [CreatedAt] < @CreatedAt
    ORDER BY [CreatedAt] DESC;

    -- Якщо не знайдено попереднього запису, встановити різницю на 0
    IF @LastCurrentValue IS NULL
    BEGIN
        SET @LastCurrentValue = @CurrentValue;
    END

    -- Розрахувати різницю
    DECLARE @DifferenceWithPreviousValue DECIMAL(32, 16);
    SET @DifferenceWithPreviousValue = @CurrentValue - @LastCurrentValue;

    -- Вставити новий запис
    INSERT INTO [dbo].[Statistic] ([Id], [CurrentValue], [DifferenceWithPreviousValue], [CreatedAt])
    VALUES (@Id, @CurrentValue, @DifferenceWithPreviousValue, @CreatedAt);
RETURN 0
