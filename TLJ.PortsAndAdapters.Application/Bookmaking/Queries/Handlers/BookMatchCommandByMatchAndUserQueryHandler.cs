using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Dapper.Queries.Handlers;
using Kitbag.Builder.CQRS.Dapper.Sql;
using TLJ.PortsAndAdapters.Application.Bookmaking.DTO;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Queries.Handlers
{
    public class BookMatchCommandByMatchAndUserQueryHandler : DapperSingleQueryHandler<BookMatchCommandByMatchAndUserQuery, BookMatchDTO>
    {
        public BookMatchCommandByMatchAndUserQueryHandler(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
        { }
        protected override string TableOrViewName => "BookMatches";
        public override async Task<BookMatchDTO> HandleAsync(BookMatchCommandByMatchAndUserQuery query)
        {
            query.SqlBuilder.Where("UserId = @UserId AND MatchId = @MatchId", new { query.MatchId, query.UserId });
            var result = await base.HandleAsync(query);
            return result;
        }

       
    }
}