using Honamic.Framework.Commands;
using Honamic.Framework.Endpoints.Web.Results;
using Honamic.Framework.Queries;
using Microsoft.AspNetCore.Mvc;
using TodoSample.Application.Contracts.Todos.Commands;
using TodoSample.Application.Contracts.Todos.Queries;

namespace TodoSample.WebApi.Todos;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder endpoints, string prefixRoute)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        prefixRoute = prefixRoute + "todos";

        var routeGroup = endpoints.MapGroup(prefixRoute)
            .WithTags("todos")
            //.RequireAuthorization()
            ;
        routeGroup.MapGet("/",
            async Task<IResult> (
                [AsParameters] GetAllTodosQuery query,
                [FromServices] IQueryBus queryBus,
                CancellationToken cancellationToken) =>
            {
                var result = await queryBus.DispatchAsync(query, cancellationToken);

                return result.ToMinimalApiResult();
            })
            .WithName(nameof(GetAllTodosQuery))
            .WithOpenApi();

        routeGroup.MapGet("/{id}",
            async Task<IResult> (
                [FromRoute] long id,
                [FromServices] IQueryBus queryBus,
                CancellationToken cancellationToken) =>
            {
                var query = new GetTodoQuery
                {
                    Id = id
                };

                var result = await queryBus.DispatchAsync(query, cancellationToken);

                return result.ToMinimalApiResult();
            })
            .WithName(nameof(GetTodoQuery))
            .WithOpenApi();

        routeGroup.MapPost("/",
              async Task<IResult> (
                  [AsParameters] CreateTodoCommand query,
                  [FromServices] ICommandBus commandBus,
                  CancellationToken cancellationToken) =>
              {
                   var result = await commandBus.DispatchAsync(query, cancellationToken);

                  return result.ToMinimalApiResult();
              })
              .WithName(nameof(CreateTodoCommand))
              .WithOpenApi();
    }
}