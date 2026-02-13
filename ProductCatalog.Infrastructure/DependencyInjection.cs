using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.Interfaces;
using ProductCatalog.Infrastructure.Repositories;

namespace ProductCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProductCatalogDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLServerConnection"), 
                    op => op.MigrationsHistoryTable("__ProductCatalogMigrationsHistory", "Catalog"))
                .AddInterceptors(new AuditInterceptor());
        });
        services.AddScoped<IProductCatalogRepository, ProductCatalogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}