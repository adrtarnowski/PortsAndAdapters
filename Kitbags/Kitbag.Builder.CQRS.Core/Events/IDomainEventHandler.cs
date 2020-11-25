using System.Threading.Tasks;
using Kitbag.Builder.Core.Domain;

namespace Kitbag.Builder.CQRS.Core.Events
{
    public interface IDomainEventHandler<TEvent> where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}