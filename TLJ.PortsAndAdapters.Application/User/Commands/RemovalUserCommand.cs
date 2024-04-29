using Kitbag.Builder.CQRS.Core.Commands;

namespace TLJ.PortsAndAdapters.Application.User.Commands;

public class RemovalUserCommand : ICommand
{
    public string? FullDomainName { get; set; }
}