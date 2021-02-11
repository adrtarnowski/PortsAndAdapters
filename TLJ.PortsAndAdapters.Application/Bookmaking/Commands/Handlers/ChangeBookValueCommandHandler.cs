using System.Threading.Tasks;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Core.Domain.Exceptions;
using Kitbag.Builder.Core.Domain.Rules;
using Kitbag.Builder.CQRS.Core.Commands;
using TLJ.PortsAndAdapters.Core.Domain.Book;
using TLJ.PortsAndAdapters.Core.Repositories;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Commands.Handlers
{
    public class ChangeBookValueCommandHandler : ICommandHandler<ChangeBookValueCommand>
    {
        private readonly IBookMatchRepository _bookMatchRepository;

        public ChangeBookValueCommandHandler(IBookMatchRepository bookMatchRepository)
        {
            _bookMatchRepository = bookMatchRepository;
        }
        
        public async Task HandleAsync(ChangeBookValueCommand command)
        {
            var currency = command.Currency ?? throw new BrokenBusinessRuleException(new RequiredValueException(nameof(command.Stake)));
            var bookType = command.BookType ?? throw new BrokenBusinessRuleException(new RequiredValueException(nameof(command.BookType)));
            
            var bookMatch = await _bookMatchRepository.FindByUserAndMatchIdsAsync(command.UserId, command.MatchId);
            if(bookMatch == null)
                throw new BrokenBusinessRuleException(new DoesNotExistException());
            bookMatch.ChangeBookValue(command.Stake, currency, bookType.ToEnum(BookType.Draw));
        }
    }
}