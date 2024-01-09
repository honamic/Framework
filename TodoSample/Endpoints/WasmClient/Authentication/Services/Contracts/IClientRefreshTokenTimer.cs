namespace Honamic.Todo.Endpoints.WasmClient.Authentication.Services.Contracts;

public interface IClientRefreshTokenTimer : IDisposable
{
    Task StartRefreshTimerAsync();
    Task StopRefreshTimerAsync();
}