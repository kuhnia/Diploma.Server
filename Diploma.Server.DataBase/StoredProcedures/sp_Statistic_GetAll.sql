CREATE PROCEDURE [dbo].[sp_Statistic_GetAll]
AS
    SELECT [Id], [CurrentValue], [DifferenceWithPreviousValue], [CreatedAt]
    FROM [dbo].[Statistic]
    ORDER BY [CreatedAt] ASC;
RETURN 0
