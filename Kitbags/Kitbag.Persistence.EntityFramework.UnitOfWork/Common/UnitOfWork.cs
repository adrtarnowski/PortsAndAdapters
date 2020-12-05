using System;
using System.Threading.Tasks;
using Kitbag.Builder.Persistence.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace Kitbag.Persistence.EntityFramework.UnitOfWork.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CommitAsync()
        {
            var count = await _context.SaveChangesAsync(false);
            return count;
        }
    }
}