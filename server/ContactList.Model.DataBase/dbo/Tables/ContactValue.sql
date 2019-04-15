CREATE TABLE [dbo].[ContactValue] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Value]      VARCHAR (100)    NOT NULL,
    [PersonId]   UNIQUEIDENTIFIER NOT NULL,
    [InsertDate] DATETIME         NOT NULL,
    [IsPhone]    BIT              CONSTRAINT [DF_ContactValue_IsPhone] DEFAULT ((0)) NOT NULL,
    [IsWhatsApp] BIT              CONSTRAINT [DF_ContactValue_IsWhatsApp] DEFAULT ((0)) NOT NULL,
    [IsEmail]    BIT              CONSTRAINT [DF_ContactValue_IsEmail] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContactValue_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);





