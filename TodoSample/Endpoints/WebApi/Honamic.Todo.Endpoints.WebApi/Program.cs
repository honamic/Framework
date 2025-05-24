using Honamic.Todo.Endpoints.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseConfigurations();

app.Run();
