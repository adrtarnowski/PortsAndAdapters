using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Kitbag.Builder.MessageBus.Common;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public class ServiceBusSubscriptionBuilder : IServiceBusSubscriptionBuilder
    {
        private readonly BusProperties _busProperties;
        private readonly ServiceBusAdministrationClient _administrationClient;
        private readonly ILogger<ServiceBusSubscriptionBuilder> _logger;

        public ServiceBusSubscriptionBuilder(
            BusProperties busProperties, 
            ServiceBusAdministrationClient administrationClient, 
            ILogger<ServiceBusSubscriptionBuilder> logger)
        {
            _administrationClient = administrationClient;
            _busProperties = busProperties;
            _logger = logger;
        }

        public async Task AddCustomRule(string subject)
        {
            try
            {
                await _administrationClient.CreateRuleAsync(_busProperties.EventTopicName,
                    _busProperties.EventSubscriptionName, new CreateRuleOptions
                    {
                        Name = subject,
                        Filter = new CorrelationRuleFilter() { Subject = subject }
                    });
            }
            catch (ServiceBusException exception)
            {
                _logger.LogInformation($"The messaging entity {subject} already exists.", exception.Message);
            }
        }

        public async Task RemoveDefaultRule()
        {
            try
            {
                await _administrationClient.DeleteRuleAsync(_busProperties.EventTopicName,
                    _busProperties.EventSubscriptionName, "$Default");
            }
            catch (ServiceBusException exception)
            {
                _logger.LogInformation($"The messaging entity has encounter an issue {exception.Message}");
            }
        }
    }
}