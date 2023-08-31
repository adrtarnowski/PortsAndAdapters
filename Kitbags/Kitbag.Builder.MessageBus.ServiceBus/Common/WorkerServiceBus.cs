using System;
using System.Threading;
using System.Threading.Tasks;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public sealed class WorkerServiceBus : IHostedService, IDisposable
    {
        private readonly IEventSubscriber _busEventSubscriber;
        private readonly ILogger<WorkerServiceBus> _logger;

        public WorkerServiceBus(
            IEventSubscriber busEventSubscriber,
            ILogger<WorkerServiceBus> logger)
        {
            _busEventSubscriber = busEventSubscriber;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting the service bus subscription listener");
            await _busEventSubscriber.RegisterOnMessageHandlerAndReceiveMessages().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stopping the service bus subscription listener");
            await _busEventSubscriber.CloseSubscriptionAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async void Dispose(bool disposing)
        {
            if (disposing)
            {
                await _busEventSubscriber.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}