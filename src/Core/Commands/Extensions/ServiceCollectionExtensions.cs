using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultCommandsServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandBus, CommandBus>();
    }
}
