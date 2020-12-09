using System;
using System.Threading.Tasks;
using Kitbag.Builder.Persistence.Core.Common;
using TLJ.PortsAndAdapters.Core.Domain.Book;

namespace TLJ.PortsAndAdapters.Core.Repositories
{
    public interface IBookMatchRepository : IRepository<BookMatch, BookMatchId>
    {
        Task<bool> AnyByUserAndMatchAsync(Guid userId, Guid matchId);
    }
}