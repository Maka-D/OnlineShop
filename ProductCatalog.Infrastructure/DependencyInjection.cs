using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProductCatalogDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLServerConnection"), 
                    op => op.MigrationsHistoryTable("__ProductCatalogMigrationsHistory", "catalog"))
                .AddInterceptors(new AuditInterceptor());
        });
        return services;
    }
}