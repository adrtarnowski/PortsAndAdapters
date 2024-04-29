using Kitbag.Builder.CQRS.Dapper.Queries;
using TLJ.PortsAndAdapters.Application.User.DTO;

namespace TLJ.PortsAndAdapters.Application.User.Queries;

public class SpecificUserQuery : DapperQuery<UserDTO>
{
    public string? UserName { get; set; }
}