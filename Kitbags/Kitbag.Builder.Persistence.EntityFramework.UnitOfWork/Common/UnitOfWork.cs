using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kitbag.Persistence.EntityFramework.UnitOfWork.Common;

public class UnitOfWork : ITransactionalUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
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

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            return null;
        _currentTransaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        return _currentTransaction;
    }
        
    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await CommitAsync();
            transaction.Commit();
        }
        catch (Exception)
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}