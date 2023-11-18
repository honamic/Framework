using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Todo.Domain.Extensions;

public static class DomainServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {

        // add domainServices
        //services.AddTransient<IXDomainService, XDomainService>();

    }
}