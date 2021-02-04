using System;
using Kitbag.Builder.MessageBus.Common;
using Kitbag.Builder.MessageBus.ServiceBus.Clients;
using Microsoft.Azure.ServiceBus;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public class BusClient : IBusClient
    {
        private readonly BusTopicConnections _busTopicConnections;
        private bool _disposed;
        public ITopicClient GetEventTopicClient() => _busTopicConnections.GetTopicClient();

        public BusClient(BusProperties busProperties)
        {
            if (busProperties.ConnectionString == null)
                throw new ArgumentNullException(nameof(busProperties.ConnectionString));
            if (busProperties.EventTopicName == null)
                throw new ArgumentNullException(nameof(busProperties.EventTopicName));
            _busTopicConnections = new BusTopicConnections(busProperties.ConnectionString, busProperties.EventTopicName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) 
                return;

            if (disposing) 
                _busTopicConnections.Dispose();

            _disposed = true;
        }

        ~BusClient()
        {
            Dispose(false);
        }
    }
}