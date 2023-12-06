## :bookmark_tabs: Audit Trail 
Kitbag provides audit trail mechanism that can track any database changes without making any unnecessary modification in existing code. Audit trail (also called audit log) provides documentary evidence of the sequence of activities that have affected at any time a specific operation. Kitbag uses the most popular ORM tool in .NET - Entity Framework Core.

## Getting Started
To use `Kitbag.Builder.Persistence.EntityFramework.Audit`, you need to add the following line in `Extension` class in the `TLJ.PortsAndAdapters.Infrastructure` project:

```
public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
{
    ...
    builder.AddEntityFramework<DatabaseContext>();
    builder.AddEntityFrameworkAuditTrail<DatabaseContext>();
    builder.AddUnitOfWork();`
    builder.Services.Decorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
    builder.Services.Decorate(typeof(ICommandHandler<>), typeof(AuditTrailCommandHandlerDecorator<>));
    ...
``` 

You have to create a dedicated table Audit Trail in your database. You can use the following script:

```
CREATE TABLE [dbo].[Audits]
(
    [Id]         [bigint] IDENTITY (1,1) NOT NULL,
    [TableName]  [nvarchar](255)         NOT NULL,
    [DateTime]   [datetime2](7)          NOT NULL,
    [OldValues]  [nvarchar](max)         NULL,
    [NewValues]  [nvarchar](max)         NULL,
    [KeyValues]  [nvarchar](255)         NOT NULL,
    [Entity]     [nvarchar](255)         NOT NULL,
    [ChangeType] [int]                   NOT NULL,

    CONSTRAINT [PK_Audits] PRIMARY KEY (Id)
);
```

## How does it work?:
When you configure Audit Trail Kitbag, each write operation based on Entity Framework saves automatically in Audits table. It defines the following columns:

- TableName (name)
- DateTime (change time)
- OldValues (JSON structure)
- NewValues (JSON stucture)
- KeyValues (entities identificators)
- Entity (name)
- ChangeType (Added - 0, Modified - 1, Deleted - 2)
