using System;
using Microsoft.Azure.ServiceBus;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public interface IBusClient : IDisposable
    {
        public ITopicClient GetEventTopicClient();
    }
}