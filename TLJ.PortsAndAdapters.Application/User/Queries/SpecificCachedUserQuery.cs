using Kitbag.Builder.CQRS.Core.Queries;
using TLJ.PortsAndAdapters.Application.User.DTO;

namespace TLJ.PortsAndAdapters.Application.User.Queries
{
    public class SpecificCachedUserQuery : IQuery<UserDTO>
    {
        public string? UserName { get; set; }
    }
}