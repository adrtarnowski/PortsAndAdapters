using System;
using Kitbag.Builder.CQRS.Core.Commands;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Commands
{
    public class CloseBookmakingCommand : ICommand
    {
        public Guid MatchId { get; set; }
        
        public Guid UserId { get; set; }
    }
}