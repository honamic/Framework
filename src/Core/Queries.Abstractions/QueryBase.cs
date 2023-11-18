
namespace Honamic.Framework.Queries;

public class AggregateQueryBase<TKey> : EntityQueryBase<TKey>
{
    public long Version { get; set; }
    public string? ModifiedSources { get; set; }
    public string? CreatedSources { get; set; }
}