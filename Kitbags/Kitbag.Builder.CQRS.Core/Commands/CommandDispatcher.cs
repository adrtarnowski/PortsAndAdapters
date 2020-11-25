using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.CQRS.Core.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendAsync<T>(T command) where T : class, ICommand
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            await handler.HandleAsync((dynamic)command);
        }
    }
}