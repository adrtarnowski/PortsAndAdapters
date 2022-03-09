using System.Threading.Tasks;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Core.Domain.Exceptions;
using Kitbag.Builder.Core.Domain.Rules;
using Kitbag.Builder.CQRS.Core.Commands;
using TLJ.PortsAndAdapters.Core.Domain.Book;
using TLJ.PortsAndAdapters.Core.Repositories;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Commands.Handlers
{
    public class BookMatchCommandHandler : ICommandHandler<BookMatchCommand>
    {
        private readonly IBookMatchRepository _bookMatchRepository;

        public BookMatchCommandHandler(IBookMatchRepository bookMatchRepository)
        {
            _bookMatchRepository = bookMatchRepository;
        }

        public async Task HandleAsync(BookMatchCommand command)
        {
            var currency = command.Currency ?? throw new BrokenBusinessRuleException(new RequiredValueException(nameof(command.Currency)));
            var bookType = command.BookType ?? throw new BrokenBusinessRuleException(new RequiredValueException(nameof(command.BookType)));
            
            if(await _bookMatchRepository.AnyByUserAndMatchAsync(command.UserId, command.MatchId))
                throw new BrokenBusinessRuleException(new DuplicateValueException());
            
            _bookMatchRepository.Add(new BookMatch(
                BookMatchId.Generate(),
                command.MatchId,
                command.UserId,
                command.Stake,
                currency,
                bookType.ToEnum(BookType.Draw)));
        }
    }
}