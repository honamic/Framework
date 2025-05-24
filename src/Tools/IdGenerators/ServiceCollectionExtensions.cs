using Honamic.Framework.Utilities.Cryptography;
using Honamic.Framework.Domain;
using IdGen;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Tools.IdGenerators;
public static class ServiceCollectionExtensions
{
    public static void AddSnowflakeIdGeneratorServices(this IServiceCollection services)
    {
        services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();

        services.AddSingleton(sp =>
        {
            var Options = new IdGeneratorOptions();
            int maxNumber = 1 << Options.IdStructure.GeneratorIdBits;
            var workerId = WorkerIdGenerator.GenerateWorkerId(maxNumber);
            return new IdGenerator(workerId, Options);
        });
    }
}