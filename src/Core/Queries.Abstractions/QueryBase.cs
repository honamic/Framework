
namespace Honamic.Framework.Queries;

public class AggregateQueryBase<TKey> : EntityQueryBase<TKey>
{
    public long Version { get; set; }
}