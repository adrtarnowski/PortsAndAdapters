using System.Threading.Tasks;
using Kitbag.Builder.Core.Domain;

namespace Kitbag.Builder.CQRS.Core.Events;

public interface IDomainEventDispatcher
{
    Task Send<T>(T @event) where T : class, IDomainEvent;
}