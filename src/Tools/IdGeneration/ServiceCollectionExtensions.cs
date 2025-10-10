using Honamic.Framework.Utilities.Cryptography;
using Honamic.Framework.Domain;
using IdGen;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Tools.IdGeneration;
public static class ServiceCollectionExtensions
{
    public static void AddSnowflakeIdGenerator(this IServiceCollection services, Action<IdGeneratorOptions>? configure = null)
    {
        services.AddSingleton(sp =>
        {
            var options = new IdGeneratorOptions();
            configure?.Invoke(options);

            int maxNumber = 1 << options.IdStructure.GeneratorIdBits;
            var workerId = WorkerIdGenerator.GenerateWorkerId(maxNumber);

            return new IdGenerator(workerId, options);
        });

        services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();
    }
}