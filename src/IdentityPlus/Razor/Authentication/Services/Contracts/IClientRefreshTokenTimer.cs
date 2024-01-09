namespace Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;

public interface IClientRefreshTokenTimer : IDisposable
{
    Task StartRefreshTimerAsync();
    Task StopRefreshTimerAsync();
}