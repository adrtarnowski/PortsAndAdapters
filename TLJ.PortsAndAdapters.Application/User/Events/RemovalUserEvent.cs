using Kitbag.Builder.Core.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace TLJ.PortsAndAdapters.Application.User.Events
{
    public class RemovalUserEvent : IntegrationEvent
    {
        public string UserName { get; }
        
        public RemovalUserEvent(string userName)
        {
            UserName = userName;
            CreationDate = SystemTime.OffsetNow();
        }
    }
}