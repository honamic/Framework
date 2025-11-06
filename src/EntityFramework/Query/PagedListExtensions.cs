using Honamic.Framework.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Honamic.Framework.EntityFramework.QueryModels;

public static class PagedListExtensions
{
    public static async Task<PagedQueryResult<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> source,
       PagedQueryFilter queryFilter,
       CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(queryFilter.GetOrderBy()))
        {
            throw new ArgumentNullException(nameof(queryFilter.OrderBy), "OderyBy is not specified to convert to a paged list.");
        }

        var totalItems = await source.CountAsync();

        var result = new PagedQueryResult<TSource>(totalItems, queryFilter.Page, queryFilter.PageSize);

        result.Items = await source
            .OrderBy(queryFilter.GetOrderBy())
            .Skip(queryFilter.SkipCount())
            .Take(queryFilter.PageSize)
            .ToListAsync(cancellationToken);

        return result;
    }

    public static Task<PagedQueryResult<TSource>> ToFilteredPagedListAsync<TSource>(this IQueryable<TSource> source,
        PagedQueryFilter queryFilter,
        CancellationToken cancellationToken = default)
    {
        return source.ApplyFilter(queryFilter).ToPagedListAsync(queryFilter, cancellationToken);
    }
}
