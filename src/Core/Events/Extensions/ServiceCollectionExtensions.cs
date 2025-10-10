using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Honamic.Framework.Events.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultEventsServices(this IServiceCollection services)
    {
        services.AddDefaultEventBusServices();
        services.AddDefaultDomainEventsDispatcherServices();
    }

    public static void AddDefaultEventBusServices(this IServiceCollection services)
    {
        services.TryAddScoped<IEventBus, EventBus>();
    }

    public static void AddDefaultDomainEventsDispatcherServices(this IServiceCollection services)
    {
        services.TryAddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.TryAddScoped<IEventStore, DisableEventStore>();
    }
}