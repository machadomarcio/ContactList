﻿/*
Deployment script for ContactList.Model.DataBase

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
/*
:setvar DatabaseName "ContactList.Model.DataBase"
:setvar DefaultFilePrefix "ContactList.Model.DataBase"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""
*/

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [master];


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)] COLLATE SQL_Latin1_General_CP1_CI_AS
GO
USE [$(DatabaseName)];


GO
PRINT N'Creating [dbo].[ContactValue]...';


GO
CREATE TABLE [dbo].[ContactValue] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Value]      VARCHAR (100)    NOT NULL,
    [ContactId]  UNIQUEIDENTIFIER NOT NULL,
    [InsertDate] DATETIME         NOT NULL,
    [IsPhone]    BIT              NOT NULL,
    [IsWhatsApp] BIT              NOT NULL,
    [IsEmail]    BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Person]...';


GO
CREATE TABLE [dbo].[Person] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [FirstName]  VARCHAR (50)     NOT NULL,
    [LastName]   VARCHAR (50)     NOT NULL,
    [InsertDate] DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[DF_ContactValue_IsPhone]...';


GO
ALTER TABLE [dbo].[ContactValue]
    ADD CONSTRAINT [DF_ContactValue_IsPhone] DEFAULT ((0)) FOR [IsPhone];


GO
PRINT N'Creating [dbo].[DF_ContactValue_IsWhatsApp]...';


GO
ALTER TABLE [dbo].[ContactValue]
    ADD CONSTRAINT [DF_ContactValue_IsWhatsApp] DEFAULT ((0)) FOR [IsWhatsApp];


GO
PRINT N'Creating [dbo].[DF_ContactValue_IsEmail]...';


GO
ALTER TABLE [dbo].[ContactValue]
    ADD CONSTRAINT [DF_ContactValue_IsEmail] DEFAULT ((0)) FOR [IsEmail];


GO
PRINT N'Creating [dbo].[FK_ContactValue_Contact]...';


GO
ALTER TABLE [dbo].[ContactValue]
    ADD CONSTRAINT [FK_ContactValue_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Person] ([Id]);


GO
PRINT N'Update complete.';


GO