using Microsoft.EntityFrameworkCore.Storage;
using ProductCatalog.Domain.Interfaces;

namespace ProductCatalog.Infrastructure.Repositories;

public class UnitOfWork :IUnitOfWork
{
    private readonly ProductCatalogDbContext _dbContext;
    private bool _disposed;
    private IDbContextTransaction? _currentTransaction;
    
    public UnitOfWork(ProductCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return; 

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            DisposeTransaction();
        }
    }

    private void DisposeTransaction()
    {
        _currentTransaction?.Dispose();
        _currentTransaction = null;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _dbContext.Dispose();
        }
        _disposed = true;
    }
}