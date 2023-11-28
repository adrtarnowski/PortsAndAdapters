using System;
using Kitbag.Builder.Core.Domain;
using TLJ.PortsAndAdapters.Core.Domain.User;

namespace TLJ.PortsAndAdapters.Core.Domain.Events
{
    public class UserCreated : DomainEventBase
    {
        public UserId UserId { get; }
        public string FullDomainName { get; }
        public DateTimeOffset CreationDate { get; }
        
        public UserCreated(UserId userId, string fullDomainName, DateTimeOffset creationDate)
        {
            UserId = userId;
            FullDomainName = fullDomainName;
            CreationDate = creationDate;
        }
    }
}