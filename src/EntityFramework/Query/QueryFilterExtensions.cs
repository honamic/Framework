using Gridify;
using Honamic.Framework.Queries;

namespace Honamic.Framework.EntityFramework.QueryModels;

public static class QueryFilterExtensions
{ 
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, QueryFilter filter)
    {
        if (string.IsNullOrWhiteSpace(filter.Filter))
            return query;

        return query.ApplyFiltering(filter.Filter);
    }

}