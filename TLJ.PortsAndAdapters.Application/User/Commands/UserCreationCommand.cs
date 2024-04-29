using Kitbag.Builder.CQRS.Core.Commands;

namespace TLJ.PortsAndAdapters.Application.User.Commands;

public class UserCreationCommand : ICommand
{
    public string? FullDomainName { get; set; }
        
    public string? UserType { get; set; }
}