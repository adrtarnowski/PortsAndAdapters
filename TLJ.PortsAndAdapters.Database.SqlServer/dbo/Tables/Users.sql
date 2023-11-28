CREATE TABLE [dbo].[Users]
(
    [Id]             [uniqueidentifier]      NOT NULL,
    [ClusteredId]    [bigint] IDENTITY (1,1) NOT NULL,
    [FullDomainName] [nvarchar](255)         NOT NULL,
    [UserType]       [int]                   NOT NULL,
    [UserStatus]     [int]                   NOT NULL,
    [CreationDate]   datetimeoffset(7)       NOT NULL,
    [LastUpdateDate] datetimeoffset(7)       NOT NULL,

    CONSTRAINT [PK_Users] PRIMARY KEY (Id)
);