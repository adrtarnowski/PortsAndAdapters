namespace Kitbag.Builder.Core.Domain;

public interface IAggregateRoot { }

public interface IAggregateRoot<TId> : IAggregateRoot
    where TId : TypedIdValueBase
{
    TId Id { get; }
}