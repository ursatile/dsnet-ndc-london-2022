CREATE DATABASE [autobarn]
GO
USE [autobarn]
GO

CREATE TABLE [dbo].[Manufacturers] (
	[Code] [varchar](32) NOT NULL PRIMARY KEY,
	[Name] [varchar](32) NOT NULL
)

CREATE TABLE [dbo].[Models](
	[Code] [varchar](32) NOT NULL PRIMARY KEY,
	[ManufacturerCode] [varchar](32) NOT NULL,
	[Name] [varchar](32) NOT NULL
)

CREATE TABLE [dbo].[Vehicles](
	[Registration] [varchar](16) NOT NULL PRIMARY KEY,
	[ModelCode] [varchar](32) NOT NULL,
	[Color] [varchar](32) NOT NULL,
	[Year] [int] NOT NULL,
)

ALTER TABLE [dbo].[Models]  WITH CHECK ADD  CONSTRAINT [FK_Models_Manufacturers] FOREIGN KEY([ManufacturerCode])
REFERENCES [dbo].[Manufacturers] ([Code])

ALTER TABLE [dbo].[Models] CHECK CONSTRAINT [FK_Models_Manufacturers]

ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicles_Models] FOREIGN KEY([ModelCode])
REFERENCES [dbo].[Models] ([Code])

ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_Models]
GO

USE [master]
GO
CREATE LOGIN autobarn WITH PASSWORD = 'autobarn', DEFAULT_DATABASE = autobarn,
CHECK_POLICY = OFF,
CHECK_EXPIRATION = OFF ;
GO

USE [autobarn]
GO

CREATE USER [autobarn] FOR LOGIN [autobarn]
GO

GO
ALTER ROLE [db_owner] ADD MEMBER [autobarn]
GO
