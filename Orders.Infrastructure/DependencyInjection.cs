using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Domain.Interfaces;
using Orders.Infrastructure.Repositories;

namespace Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrdersDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLServerConnection"), 
                    op => op.MigrationsHistoryTable("__OrdersMigrationsHistory", "Orders"))
                .AddInterceptors(new AuditInterceptor());
        });
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}