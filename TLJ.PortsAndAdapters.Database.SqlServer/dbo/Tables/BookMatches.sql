CREATE TABLE [dbo].[BookMatches]
(
	[Id] [uniqueidentifier] NOT NULL,
	[ClusteredId] [bigint] IDENTITY(1,1) NOT NULL,
	[MatchId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[BookType] [int] NOT NULL,
	[Value] DECIMAL(8, 2) NOT NULL,
	[Currency] CHAR(6) NOT NULL,
	[CreateDate] datetimeoffset(7) NOT NULL,
    [IsClose] BIT NOT NULL DEFAULT(0),

	CONSTRAINT [PK_BookMatches] PRIMARY KEY NONCLUSTERED ([Id] ASC),
	CONSTRAINT [AK_BookMatches_ClusteredId] UNIQUE NONCLUSTERED ([ClusteredId] ASC)
)
