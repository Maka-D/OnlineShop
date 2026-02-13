
using Orders.Domain.Interfaces;

namespace Orders.Infrastructure.Repositories;


public class UnitOfWork :IUnitOfWork
{
    private readonly OrdersDbContext _dbContext;
    private bool _disposed;
    
    public UnitOfWork(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
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