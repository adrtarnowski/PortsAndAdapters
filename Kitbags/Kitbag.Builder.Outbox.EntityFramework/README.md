# :inbox_tray: Outbox Pattern

`Kitbag.Builder.Outbox` and `Kitbag.Builder.Outbox.EntityFramework` (used EF as a event log) are libraries that provide Outbox Pattern functionality (more about pattern [here](https://microservices.io/patterns/data/transactional-outbox.html)).

The whole implementation idea behind it is to have a separate table in the database that will store all events that should be published. The events are stored in the same transaction as the business data (to ensure atomicity, the decorator pattern is used to wrap the handlers in a transaction: `UnitOfWorkCommandHandlerDecorator` and `OutboxHandlerDecorator`). 
The events are published by a separate process that reads the events from the database and publishes them to the message bus. The process that publishes the events is idempotent and transactional. 
It reads the events from the database and publishes them to the message bus in a transaction. After the events are published, it marks them as `processed` in the database (exactly-once delivery)

![image](./assets/outbox_pattern.png)

## Getting Started
To use `Kitbag.Builder.Outbox.EntityFramework`, you need to add the following line in `Extension` class in the `TLJ.PortsAndAdapters.Infrastructure` project:

```
public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
{
    ...
    builder.AddEntityFramework<DatabaseContext>();
    builder.AddEntityFrameworkOutbox<DatabaseContext>();
    ...
    builder.Services.Decorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
    builder.Services.Decorate(typeof(ICommandHandler<>), typeof(OutboxHandlerDecorator<>));
    ...
```

## How to use

To insert an event into the outbox table, you need to do the following things:
- Create a `OutboxMessages` table
```
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
```
- Create a `OutboxConfiguration` class that implements `IEntityTypeConfiguration<OutboxMessage>` interface
- Update your Database context by adding a `protected internal DbSet<OutboxMessage>? OutboxMessages { get; set; }` property
- Emit the event in aggregate by using `AddDomainEvent(new UserCreated(Id, FullDomainName, CreationDate));` method