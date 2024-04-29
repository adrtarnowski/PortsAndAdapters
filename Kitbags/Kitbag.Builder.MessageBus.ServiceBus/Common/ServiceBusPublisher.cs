using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Kitbag.Builder.MessageBus.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common;

public class ServiceBusPublisher<T> : IBusPublisher<T> where T : IIntegrationEvent
{
    private readonly ILogger<ServiceBusPublisher<T>> _logger;
    private readonly ServiceBusSender _sender;

    public ServiceBusPublisher(
        IAzureClientFactory<ServiceBusSender> serviceBusSenderFactory,
        ILogger<ServiceBusPublisher<T>> logger)
    {
        var eventType = MessageBus.Extensions.GetEventFor<T>();
        _sender = serviceBusSenderFactory.CreateClient(eventType);
        _logger = logger;
    }

    public async Task PublishEventAsync(T payload, string? sessionId)
    {
        var eventType = MessageBus.Extensions.GetEventFor<T>();
        try
        {
            var message = CreateBusMessage(eventType, payload, sessionId);
            await _sender.SendMessageAsync(message).ConfigureAwait(false); 
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    private ServiceBusMessage CreateBusMessage(string? messageSubject, object payload, string? sessionId)
    {
        string data = payload is string ? (string) payload : JsonConvert.SerializeObject(payload);
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(data));
        message.SessionId = sessionId ?? Guid.NewGuid().ToString();
        message.Subject = messageSubject;
        return message;
    }
}