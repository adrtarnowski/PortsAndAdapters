using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TLJ.PortsAndAdapters.Core.Domain.User;
using TLJ.PortsAndAdapters.Core.Repositories;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Repositories;

public class UserRepository : DatabaseRepository<User, UserId>, IUserRepository
{
    public UserRepository(DatabaseContext context) : base(context) { }
        
    public Task<User?> FindByUserName(string userName)
    {
        return  _context.Set<User>()
            .Where(x => x.FullDomainName == userName).SingleOrDefaultAsync();
    }
}