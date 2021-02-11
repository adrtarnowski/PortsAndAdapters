using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TLJ.PortsAndAdapters.Core.Domain.Book;
using TLJ.PortsAndAdapters.Core.Repositories;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Repositories
{
    public class BookMatchRepository : DatabaseRepository<BookMatch, BookMatchId>, IBookMatchRepository
    {
        public BookMatchRepository(DatabaseContext context) : base(context) { }
        
        public Task<bool> AnyByUserAndMatchAsync(Guid userId, Guid matchId)
        {
            return _context.Set<BookMatch>()
                .Where(x => x.UserId == userId && x.MatchId == matchId).AnyAsync();
        }
        
        public Task<BookMatch> FindByUserAndMatchIdsAsync(Guid userId, Guid matchId)
        {
            return  _context.Set<BookMatch>()
                .Where(x => x.UserId == userId && x.MatchId == matchId).SingleOrDefaultAsync();
        }
    }
}