CREATE TABLE [dbo].[Statistic]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [CurrentValue] DECIMAL(32, 16) NOT NULL, 
    [DifferenceWithPreviousValue] DECIMAL(32, 16) NOT NULL, 
    [CreatedAt] DATETIME NOT NULL
)
