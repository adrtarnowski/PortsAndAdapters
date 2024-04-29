using System.Threading.Tasks;
using Kitbag.Builder.Persistence.Core.Common;
using TLJ.PortsAndAdapters.Core.Domain.User;

namespace TLJ.PortsAndAdapters.Core.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> FindByUserName(string userName);
}