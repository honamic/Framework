using Microsoft.Extensions.Logging;

namespace Honamic.Framework.Facade;

public abstract class BaseFacade : IBaseFacade
{
    public ILogger Logger { get; }


    public BaseFacade(ILogger logger)
    {
        Logger = logger;
    }
}


public interface IBaseFacade
{
    ILogger Logger { get; }
}