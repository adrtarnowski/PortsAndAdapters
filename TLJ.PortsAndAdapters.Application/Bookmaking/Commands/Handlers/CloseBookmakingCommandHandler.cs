using System;
using System.Threading.Tasks;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Core.Domain.Exceptions;
using Kitbag.Builder.Core.Domain.Rules;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.MessageBus.Common;
using TLJ.PortsAndAdapters.Application.Bookmaking.Events;
using TLJ.PortsAndAdapters.Core.Repositories;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Commands.Handlers
{
    public class CloseBookmakingCommandHandler : ICommandHandler<CloseBookmakingCommand>
    {
        private readonly IBookMatchRepository _bookMatchRepository;
        private readonly IBusPublisher _busPublisher;

        public CloseBookmakingCommandHandler(
            IBookMatchRepository bookMatchRepository, 
            IBusPublisher busPublisher)
        {
            _bookMatchRepository = bookMatchRepository;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(CloseBookmakingCommand command)
        {
            var bookMatch = await _bookMatchRepository.FindByUserAndMatchIdsAsync(command.UserId, command.MatchId);
            if(bookMatch == null)
                throw new BrokenBusinessRuleException(new DoesNotExistException());
            bookMatch.Close();
            
            //TODO: Add Ooutbox pattern
            await _busPublisher.PublishEventAsync(
                new CloseBookmakingEvent(
                    Guid.NewGuid(), 
                    SystemTime.Now(), 
                    command.MatchId, 
                    command.UserId));
        }
    }
}