using Honamic.Framework.Queries.GridColumns;

namespace Honamic.Framework.Queries;

public abstract class AggregateQueryResult<TKey> : EntityQueryResult<TKey>
{
    [GridColumn(GenerateField = false)]
    public virtual long? Version { get; set; }
}
