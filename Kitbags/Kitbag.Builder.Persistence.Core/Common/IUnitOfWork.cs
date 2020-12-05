using System.Threading.Tasks;

namespace Kitbag.Builder.Persistence.Core.Common
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}