using System.Threading.Tasks;
using Kitbag.Builder.Core.Domain.Exceptions;
using Kitbag.Builder.Core.Domain.Rules;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.MessageBus.Common;
using TLJ.PortsAndAdapters.Application.User.Events;
using TLJ.PortsAndAdapters.Core.Repositories;

namespace TLJ.PortsAndAdapters.Application.User.Commands.Handlers
{
    public class RemovalCommandHandler : ICommandHandler<RemovalUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBusPublisher<RemovalUserEvent> _busPublisher;

        public RemovalCommandHandler(
            IUserRepository userRepository, 
            IBusPublisher<RemovalUserEvent> busPublisher)
        {
            _userRepository = userRepository;
            _busPublisher = busPublisher;
        }

        public async Task Handle(RemovalUserCommand command)
        {
            if (string.IsNullOrEmpty(command.FullDomainName))
                throw new BrokenBusinessRuleException(new RequiredValueException(nameof(command.FullDomainName)));
            if(await _userRepository.FindByUserName(command.FullDomainName) != null)
                throw new BrokenBusinessRuleException(new DuplicateValueException());
            
            await _busPublisher.PublishEventAsync(
                new RemovalUserEvent(
                    command.FullDomainName));
        }
    }
}