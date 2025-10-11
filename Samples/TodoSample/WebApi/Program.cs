using Honamic.Framework.Application.Extensions;
using Honamic.Framework.Domain;
using Honamic.Framework.Endpoints.Web.Authorization;
using Honamic.Framework.Endpoints.Web.Extensions;
using Honamic.Framework.Tools.IdGeneration;
using TodoSample.Application.Extensions;
using TodoSample.Persistence.Extensions;
using TodoSample.QueryModels.EntityFramework.Extensions;
using TodoSample.WebApi;
using TodoSample.WebApi.Todos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddTodoServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

AddTodoWebApplication(app);

app.Run();


void AddTodoServices(IServiceCollection services, ConfigurationManager configuration)
{
    var sqlServerConnection = configuration.GetConnectionString("SqlServerConnectionString");

    services.AddDefaultApplicationsServices();
    services.AddSnowflakeIdGenerator();

    services.AddTodoApplicationServices();

    services.AddTodoPersistenceServices(sqlServerConnection!);

    services.AddTodoQueryModelsServices(sqlServerConnection!);

    services.AddDefaultUserContextService<DefaultUserContext>();
    services.AddScoped<IAuthorization, DefaultAuthorization>();

}


void AddTodoWebApplication(WebApplication app)
{
    app.MapTodoEndpoints("/api/");
}
