using System;
using System.Text;
using System.Threading.Tasks;
using Kitbag.Builder.MessageBus.Common;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public class ServiceBusPublisher : IBusPublisher
    {
        private readonly ILogger<ServiceBusPublisher> _logger;
        private readonly IBusClient _busClient;

        public ServiceBusPublisher(
            IBusClient busClient, 
            ILogger<ServiceBusPublisher> logger)
        {
            _busClient = busClient;
            _logger = logger;
        }

        public async Task PublishEventAsync(object payload, string messageLabel, string? sessionId)
        {
            var message = CreateBusMessage(messageLabel, payload, sessionId);

            try
            {
                await _busClient.GetEventTopicClient().SendAsync(message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        private Message CreateBusMessage(string? messageLabel, object payload, string? sessionId)
        {
            string data = payload is string ? (string) payload : JsonConvert.SerializeObject(payload);
            var message = new Message(Encoding.UTF8.GetBytes(data));
            message.Label = messageLabel;
            message.SessionId = sessionId ?? Guid.NewGuid().ToString();
            return message;
        }
    }
}