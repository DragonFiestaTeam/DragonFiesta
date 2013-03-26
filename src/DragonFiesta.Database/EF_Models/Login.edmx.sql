
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 09/03/2012 21:16:05
-- Generated from EDMX file: D:\Coding\C#\Projects\DFR-GIT\src\DragonFiesta.Login\Database\EFModel\Login.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [df_login];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_access_level]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[accounts] DROP CONSTRAINT [FK_access_level];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[access_levels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[access_levels];
GO
IF OBJECT_ID(N'[dbo].[accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[accounts];
GO
IF OBJECT_ID(N'[dbo].[hashes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hashes];
GO
IF OBJECT_ID(N'[dbo].[versions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[versions];
GO
IF OBJECT_ID(N'[LoginModelStoreContainer].[v_Auth]', 'U') IS NOT NULL
    DROP TABLE [LoginModelStoreContainer].[v_Auth];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'access_levels'
CREATE TABLE [dbo].[access_levels] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] VARCHAR(40)  NOT NULL,
    [can_login] BIT  NOT NULL
);
GO

-- Creating table 'accounts'
CREATE TABLE [dbo].[accounts] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [name] VARCHAR(40)  NOT NULL,
    [password] VARCHAR(40)  NOT NULL,
    [access_level] bigint  NOT NULL
);
GO

-- Creating table 'hashes'
CREATE TABLE [dbo].[hashes] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [hash_string] VARCHAR(40)  NOT NULL,
    [allow_login] BIT  NOT NULL
);
GO

-- Creating table 'versions'
CREATE TABLE [dbo].[versions] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [year] int  NOT NULL,
    [version_number] int  NOT NULL,
    [allowed] BIT  NOT NULL
);
GO

-- Creating table 'v_Auth'
CREATE TABLE [dbo].[v_Auth] (
    [id] bigint  NOT NULL,
    [name] VARCHAR(40)  NOT NULL,
    [password] VARCHAR(40)  NOT NULL,
    [can_login] BIT  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'access_levels'
ALTER TABLE [dbo].[access_levels]
ADD CONSTRAINT [PK_access_levels]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'accounts'
ALTER TABLE [dbo].[accounts]
ADD CONSTRAINT [PK_accounts]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'hashes'
ALTER TABLE [dbo].[hashes]
ADD CONSTRAINT [PK_hashes]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'versions'
ALTER TABLE [dbo].[versions]
ADD CONSTRAINT [PK_versions]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id], [name], [password], [can_login] in table 'v_Auth'
ALTER TABLE [dbo].[v_Auth]
ADD CONSTRAINT [PK_v_Auth]
    PRIMARY KEY CLUSTERED ([id], [name], [password], [can_login] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [access_level] in table 'accounts'
ALTER TABLE [dbo].[accounts]
ADD CONSTRAINT [FK_access_level]
    FOREIGN KEY ([access_level])
    REFERENCES [dbo].[access_levels]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_access_level'
CREATE INDEX [IX_FK_access_level]
ON [dbo].[accounts]
    ([access_level]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------