using System;
using Kitbag.Builder.CQRS.Core.Commands;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Commands
{
    public class BookMatchCommand : ICommand
    {
        public Guid MatchId { get; set; }
        
        public Guid UserId { get; set; }
        
        public decimal Stake { get; set; }
        
        public string? Currency { get; set; }
        
        public string? BookType { get; set; }
    }
}