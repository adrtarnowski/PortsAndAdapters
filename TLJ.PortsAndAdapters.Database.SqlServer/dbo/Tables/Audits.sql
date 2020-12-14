CREATE TABLE [dbo].[Audits](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](255) NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[OldValues] [nvarchar](max) NULL,
	[NewValues] [nvarchar](max) NULL,
	[CorrelationId] [nvarchar](255) NULL,
	[ChangeContext] [nvarchar](255) NULL,
	[KeyValues] [nvarchar](255) NOT NULL,
	[Entity] [nvarchar](255) NOT NULL,
	[ChangeType] [int] NOT NULL,

	CONSTRAINT [PK_Audits] PRIMARY KEY (Id) 
)