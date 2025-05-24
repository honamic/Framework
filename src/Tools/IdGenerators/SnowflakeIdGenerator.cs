using Honamic.Framework.Domain;
using IdGen;

namespace Honamic.Framework.Tools.IdGenerators;

internal class SnowflakeIdGenerator : IIdGenerator
{
    private readonly IdGenerator _idGenerator;

    public SnowflakeIdGenerator(IdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public long GetNewId()
    {
        return _idGenerator.CreateId();
    }
}