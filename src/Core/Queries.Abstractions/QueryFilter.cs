namespace Honamic.Framework.Queries;

public abstract class QueryFilter
{
    public string? OrderBy { get; set; }

    public string? Filter { get; set; }

    public string? Keyword { get; set; }

    public TimeSpan? TimeZoneOffset { get; set; }

    protected abstract string DefaultOrderBy { get; }

    public TimeSpan GetEffectiveOffset() => TimeZoneOffset ?? TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);

    public string GetOrderBy() => string.IsNullOrWhiteSpace(OrderBy) ? DefaultOrderBy : OrderBy;

    public static string OrderByDesc(string columnName) => $"{columnName} desc";
}