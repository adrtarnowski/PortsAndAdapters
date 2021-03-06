﻿CREATE TABLE [dbo].[BookMatches]
(
	[Id] [uniqueidentifier] NOT NULL,
	[ClusteredId] [bigint] IDENTITY(1,1) NOT NULL,
	[MatchId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[BookType] [int] NOT NULL,
	[Stake] DECIMAL(8, 2) NOT NULL,
	[Currency] CHAR(3) NOT NULL,
	[CreateDate] datetimeoffset(7) NOT NULL,

	CONSTRAINT [PK_BookMatches] PRIMARY KEY NONCLUSTERED ([Id] ASC),
	CONSTRAINT [AK_BookMatches_ClusteredId] UNIQUE NONCLUSTERED ([ClusteredId] ASC)
)