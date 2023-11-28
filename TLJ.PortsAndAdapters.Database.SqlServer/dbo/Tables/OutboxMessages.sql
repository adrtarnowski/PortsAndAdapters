CREATE TABLE [dbo].[OutboxMessages]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [BatchId] UNIQUEIDENTIFIER NOT NULL,
    [CreationDate] DATETIME2 NOT NULL,
    [ProcessedDate] DATETIME2 NULL,
    [Payload] NVARCHAR(4000) NOT NULL,
    [Type] NVARCHAR(300) NOT NULL,
    [Discriminator] NVARCHAR(10) NOT NULL,
    
    CONSTRAINT PK_OutboxMessages PRIMARY KEY (Id)
);