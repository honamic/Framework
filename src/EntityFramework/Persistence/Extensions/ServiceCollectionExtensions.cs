using Honamic.Framework.Domain;
using Honamic.Framework.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Persistence.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWorkByEntityFramework<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {

        services.AddScoped<DbContext>((sp) => sp.GetRequiredService<TDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IDomainEventDetector, DomainEventDetector>();

        return services;
    }
}