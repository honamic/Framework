using Honamic.Framework.Domain;
using Honamic.Framework.Events;
using Honamic.Framework.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.EntityFramework.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWorkByEntityFramework<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {

        services.AddKeyedScoped<DbContext>(DomainConstants.PersistenceDbContextKey, (sp, key) => sp.GetRequiredService<TDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IDomainEventDetector, DomainEventDetector>();

        return services;
    }
}