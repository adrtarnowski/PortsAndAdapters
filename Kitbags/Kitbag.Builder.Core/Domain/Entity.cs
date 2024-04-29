using System.Collections.Generic;
using Kitbag.Builder.Core.Domain.Exceptions;

namespace Kitbag.Builder.Core.Domain;

public abstract class Entity
{
    private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void CheckRule(IBusinessRule rule)
    {
        if (!rule.IsValid())
        {
            throw new BrokenBusinessRuleException(rule);
        }
    }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}