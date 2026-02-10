using Microsoft.EntityFrameworkCore;

namespace Orders.Infrastructure;

public class OrdersDbContext :DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        
    }
}