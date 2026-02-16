using Orders.Domain.Models;

namespace Orders.Domain.Interfaces;

public interface IOrdersRepository
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId, int pageNumber = 1, int pageSize = 10);
    Task<Order?> GetByIdAsync(int id);
    Task<bool> ExistsByIdempotencyKeyAsync(Guid key);
    void Add(Order order);
    void Update(Order order);
}