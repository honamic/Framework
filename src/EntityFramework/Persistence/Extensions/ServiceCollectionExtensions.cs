using Honamic.Framework.Domain;
using Honamic.Framework.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Persistence.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWorkByEntityFramework(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IDomainEventDetector, DomainEventDetector>();

        return services;
    }
}