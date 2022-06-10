using System;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Events
{
    public class CloseBookmakingEvent : IntegrationEvent
    {
        public Guid MatchId { get; }
        
        public Guid UserId { get; }
        
        public CloseBookmakingEvent(Guid id, DateTime creationDate, Guid matchId, Guid userId)
        {
            Id = id;
            CreationDate = creationDate;
            MatchId = matchId;
            UserId = userId;
        }
    }
}