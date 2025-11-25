
namespace Honamic.Framework.Queries;

public class AggregateRootQueryBase<TKey> : EntityQueryBase<TKey>
{
    public long Version { get; set; }
}