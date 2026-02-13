using Microsoft.EntityFrameworkCore;
using Orders.Domain.Interfaces;
using Orders.Domain.Models;

namespace Orders.Infrastructure.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrdersDbContext _dbContext;

    public OrdersRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId, int pageSize, int pageNumber)
    {
        return await _dbContext.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbContext.Orders.FindAsync(id);
    }

    public async Task<bool> ExistsByIdempotencyKeyAsync(Guid key)
    {
        return await _dbContext.Orders.AnyAsync(o => o.IdempotencyKey == key);
    }

    public void Add(Order order)
    {
        _dbContext.Orders.Add(order);
    }
}