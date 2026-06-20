using Honamic.Framework.Queries.GridColumns;
using System.Text.Json.Serialization;

namespace Honamic.Framework.Queries;

public abstract class QueryResultBase
{
    [GridColumn(GenerateField = false)]
    [JsonIgnore]
    public static TimeSpan? TimeZoneOffset { get; set; }

    public TimeSpan EffectiveOffset() => TimeZoneOffset ?? TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);

}