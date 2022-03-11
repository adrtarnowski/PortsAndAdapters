using System;
using Microsoft.Azure.ServiceBus;

namespace Kitbag.Builder.MessageBus.ServiceBus.Clients
{
    public interface IBusClient : IDisposable
    {
        public ITopicClient GetEventTopicClient();
    }
}