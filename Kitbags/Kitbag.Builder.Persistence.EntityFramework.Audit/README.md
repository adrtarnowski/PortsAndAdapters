## Audit Trail :bookmark_tabs:
Kitbag provides audit trail mechanism that can track any database changes without making any unnecessary modification in existing code. Audit trail (also called audit log) provides documentary evidence of the sequence of activities that have affected at any time a specific operation. Kitbag uses the most popular ORM tool in .NET - Entity Framework Core.
 
## How to use:
Using Audit Trail with Entity Framework Core requires register the following Kitbags in *Api* or *Infrastructure* layer:

- **Register Entity Framework**
  `builder.AddEntityFramework<DatabaseContext>();`
- **Register Audit Trail**
  `builder.AddEntityFrameworkAuditTrail<DatabaseContext>();`
- **Register Unit of Work**
`builder.AddUnitOfWork();`
- **Decorate Unit of Work and Audit Trail**
  `builder.Services.Decorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));`
  and
  `builder.Services.Decorate(typeof(ICommandHandler<>), typeof(AuditTrailCommandHandlerDecorator<>));`

## How does it work?:
When you configure Audit Trail Kitbag, each write operation based on Entity Framework saves automatically in Audits table. It defines the following columns:

- TableName (name)
- DateTime (change time)
- OldValues (JSON structure)
- NewValues (JSON stucture)
- KeyValues (entities identificators)
- Entity (name)
- ChangeType (Added - 0, Modified - 1, Deleted - 2)
