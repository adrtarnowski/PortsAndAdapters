using System.Collections.Generic;
using Kitbag.Builder.Core.Domain;

namespace Kitbag.Persistence.EntityFramework.UnitOfWork.Common
{
    public interface IDomainEventsAccessor
    {
        List<IDomainEvent> GetDomainEvents();
        void ClearAllDomainEvents();
    }
}