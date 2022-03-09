using System;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Events
{
    public class CloseBookmakingEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        
        public DateTime CreationDate { get; private set; }
        
        public Guid MatchId { get; private set; }
        
        public Guid UserId { get; private set; }
        
        public CloseBookmakingEvent(Guid id, DateTime creationDate, Guid matchId, Guid userId)
        {
            Id = id;
            CreationDate = creationDate;
            MatchId = matchId;
            UserId = userId;
        }
    }
}