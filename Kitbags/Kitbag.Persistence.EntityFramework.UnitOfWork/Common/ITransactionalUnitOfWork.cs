using System.Threading.Tasks;
using Kitbag.Builder.Persistence.Core.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kitbag.Persistence.EntityFramework.UnitOfWork.Common
{
    public interface ITransactionalUnitOfWork : IUnitOfWork
    {
        bool HasActiveTransaction { get; }
        Task<IDbContextTransaction?> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        void RollbackTransaction();
    }
}