ALTER TABLE [dbo].[BookMatches] ADD IsClose bit default (0);

UPDATE [dbo].[BookMatches] SET IsClose = 0;