CREATE TABLE [dbo].[City]
(
	[Id] INT IDENTITY(1, 1) PRIMARY KEY,
    [Name] NVARCHAR(50) NULL, 
    [State] NVARCHAR(50) NULL, 
    [Country] NVARCHAR(50) NULL, 
    [Rating] INT NULL, 
    [DateEstablished] DATETIME NULL, 
    [Population] BIGINT NULL
)
