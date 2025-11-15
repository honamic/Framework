using Honamic.Framework.Application.CommandHandlerDecorators;
using Honamic.Framework.Application.QueryHandlerDecorators;
using Honamic.Framework.Commands;
using Honamic.Framework.Commands.Extensions;
using Honamic.Framework.Domain.Extensions;
using Honamic.Framework.Events;
using Honamic.Framework.Events.Extensions;
using Honamic.Framework.Queries;
using Honamic.Framework.Queries.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Honamic.Framework.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultApplicationsServices(this IServiceCollection services)
    {
        services.AddDefaultDomainServices();
        services.AddDefaultEventsServices();
        services.AddDefaultCommandsServices();
        services.AddDefaultQueriesServices();

        return services;
    }
}