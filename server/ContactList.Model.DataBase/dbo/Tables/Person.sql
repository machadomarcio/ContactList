CREATE TABLE [dbo].[Person] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [FirstName]  VARCHAR (50)     NOT NULL,
    [LastName]   VARCHAR (50)     NOT NULL,
    [InsertDate] DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

