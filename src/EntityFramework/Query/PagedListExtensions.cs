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

        if (queryFilter.DisablePaging == true)
        {
            var items = await source.ToListAsync(cancellationToken);
            return new PagedQueryResult<TSource>(items.Count, 1, items.Count)
            {
                Items = items
            };
        }

        if (string.IsNullOrWhiteSpace(queryFilter.GetOrderBy()))
        {
            throw new ArgumentNullException(nameof(queryFilter.OrderBy), "OderyBy is not specified to convert to a paged list.");
        }

        var orderedQuery = source.OrderBy(queryFilter.GetOrderBy());
        var totalItems = await source.CountAsync(cancellationToken);
        var result = new PagedQueryResult<TSource>(totalItems, queryFilter.Page, queryFilter.PageSize);

        result.Items = await orderedQuery
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
