
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/02/2019 11:15:16
-- Generated from EDMX file: C:\Users\Egor\Documents\Visual Studio 2017\Projects\Schools\Schools\Models\SchoolsModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SchoolsDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_StudentClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentSet] DROP CONSTRAINT [FK_StudentClass];
GO
IF OBJECT_ID(N'[dbo].[FK_SchoolClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClassSet] DROP CONSTRAINT [FK_SchoolClass];
GO
IF OBJECT_ID(N'[dbo].[FK_ClassOperationStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClassOperationSet] DROP CONSTRAINT [FK_ClassOperationStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_ClassOperationClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClassOperationSet] DROP CONSTRAINT [FK_ClassOperationClass];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SchoolSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SchoolSet];
GO
IF OBJECT_ID(N'[dbo].[ClassSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClassSet];
GO
IF OBJECT_ID(N'[dbo].[StudentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentSet];
GO
IF OBJECT_ID(N'[dbo].[ClassOperationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClassOperationSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SchoolSet'
CREATE TABLE [dbo].[SchoolSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ClassSet'
CREATE TABLE [dbo].[ClassSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL,
    [Letter] nvarchar(max)  NOT NULL,
    [SchoolId] int  NOT NULL
);
GO

-- Creating table 'StudentSet'
CREATE TABLE [dbo].[StudentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Patronymic] nvarchar(max)  NOT NULL,
    [Birthday] datetime  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Class_Id] int  NULL
);
GO

-- Creating table 'ClassOperationSet'
CREATE TABLE [dbo].[ClassOperationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [OperationType] int  NOT NULL,
    [Student_Id] int  NOT NULL,
    [Class_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SchoolSet'
ALTER TABLE [dbo].[SchoolSet]
ADD CONSTRAINT [PK_SchoolSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClassSet'
ALTER TABLE [dbo].[ClassSet]
ADD CONSTRAINT [PK_ClassSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentSet'
ALTER TABLE [dbo].[StudentSet]
ADD CONSTRAINT [PK_StudentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClassOperationSet'
ALTER TABLE [dbo].[ClassOperationSet]
ADD CONSTRAINT [PK_ClassOperationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Class_Id] in table 'StudentSet'
ALTER TABLE [dbo].[StudentSet]
ADD CONSTRAINT [FK_StudentClass]
    FOREIGN KEY ([Class_Id])
    REFERENCES [dbo].[ClassSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentClass'
CREATE INDEX [IX_FK_StudentClass]
ON [dbo].[StudentSet]
    ([Class_Id]);
GO

-- Creating foreign key on [SchoolId] in table 'ClassSet'
ALTER TABLE [dbo].[ClassSet]
ADD CONSTRAINT [FK_SchoolClass]
    FOREIGN KEY ([SchoolId])
    REFERENCES [dbo].[SchoolSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SchoolClass'
CREATE INDEX [IX_FK_SchoolClass]
ON [dbo].[ClassSet]
    ([SchoolId]);
GO

-- Creating foreign key on [Student_Id] in table 'ClassOperationSet'
ALTER TABLE [dbo].[ClassOperationSet]
ADD CONSTRAINT [FK_ClassOperationStudent]
    FOREIGN KEY ([Student_Id])
    REFERENCES [dbo].[StudentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClassOperationStudent'
CREATE INDEX [IX_FK_ClassOperationStudent]
ON [dbo].[ClassOperationSet]
    ([Student_Id]);
GO

-- Creating foreign key on [Class_Id] in table 'ClassOperationSet'
ALTER TABLE [dbo].[ClassOperationSet]
ADD CONSTRAINT [FK_ClassOperationClass]
    FOREIGN KEY ([Class_Id])
    REFERENCES [dbo].[ClassSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClassOperationClass'
CREATE INDEX [IX_FK_ClassOperationClass]
ON [dbo].[ClassOperationSet]
    ([Class_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------