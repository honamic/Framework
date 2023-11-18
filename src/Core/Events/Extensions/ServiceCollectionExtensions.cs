using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Honamic.Framework.Events.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultEventssServices(this IServiceCollection services)
    {

    }

    public static void AddDefaultEventBusServices(this IServiceCollection services)
    {
        services.TryAddScoped<IEventBus, EventBus>();
    }

    public static void AddDefaultDomainEventsDispatcherServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
    }
}
