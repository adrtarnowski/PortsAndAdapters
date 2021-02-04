using System;
using Kitbag.Builder.Core.Common;
using Microsoft.Azure.ServiceBus;

namespace Kitbag.Builder.MessageBus.ServiceBus.Clients
{
    internal class BusTopicConnections : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _topicName;
        private ITopicClient _topicClient;
        private bool _disposed;

        public BusTopicConnections(string connectionString, string topicName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            _topicClient = new TopicClient(_connectionString, _topicName);
        }

        public ITopicClient GetTopicClient()
        {
            if (_topicClient.IsClosedOrClosing)
                _topicClient = new TopicClient(_connectionString, _topicName);
            return _topicClient;
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
            {
                if (!_topicClient.IsClosedOrClosing) 
                    AsyncHelper.RunSync(() => _topicClient.CloseAsync());
            }
            _disposed = true;
        }

        ~BusTopicConnections()
        {
            Dispose(false);
        }
    }
}