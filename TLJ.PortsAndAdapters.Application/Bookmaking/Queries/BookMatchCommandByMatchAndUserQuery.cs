using System;
using Kitbag.Builder.CQRS.Dapper.Queries;
using TLJ.PortsAndAdapters.Application.Bookmaking.DTO;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Queries
{
    public class BookMatchCommandByMatchAndUserQuery : DapperQuery<BookMatchDTO>
    {
        public Guid MatchId { get; set; }
        
        public Guid UserId { get; set; } 
    }
}