--AspNetRoleClaims è AspNetUserClaims
USE [OnlineShop.Users]
GO

ALTER TABLE [dbo].[AspNetRoleClaims] ADD Id_new INT IDENTITY(1, 1)
GO
ALTER TABLE [dbo].[AspNetRoleClaims] DROP CONSTRAINT [PK_AspNetRoleClaims] WITH ( ONLINE = OFF ) --write here contraint name
GO
ALTER TABLE [dbo].[AspNetRoleClaims] DROP COLUMN Id
GO
EXEC sp_rename 'AspNetRoleClaims.Id_new', 'Id', 'Column';
ALTER TABLE [dbo].[AspNetRoleClaims] ADD PRIMARY KEY (Id)
GO

ALTER TABLE [dbo].[AspNetUserClaims] ADD Id_new INT IDENTITY(1, 1)
GO
ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [PK__AspNetUs__3214EC07DB2E1A91] WITH ( ONLINE = OFF ) --write here contraint name
GO
ALTER TABLE [dbo].[AspNetUserClaims] DROP COLUMN Id
GO
EXEC sp_rename 'AspNetUserClaims.Id_new', 'Id', 'Column';
ALTER TABLE [dbo].[AspNetUserClaims] ADD PRIMARY KEY (Id)
GO
