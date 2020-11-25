using System.Threading;
using System.Threading.Tasks;

namespace Kitbag.Builder.Core.Domain
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}