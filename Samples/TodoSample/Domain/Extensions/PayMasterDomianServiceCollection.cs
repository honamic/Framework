using Microsoft.Extensions.DependencyInjection;

namespace TodoSample.Domain.Extensions;

public static class TodoDomianServiceCollection
{ 
    public static IServiceCollection AddTodoDomainServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
 

        return services;
    }
}