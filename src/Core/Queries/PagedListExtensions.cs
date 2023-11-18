using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Honamic.Framework.Queries;

public static class PagedListExtensions
{
    public static async Task<PagedQueryResult<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> source,
       PagedQueryFilter request,
       CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request.OrderBy is null)
        {
            throw new ArgumentNullException(nameof(request.OrderBy), "OderyBy is not specified to convert to a paged list.");
        }

        var totalItems = await source.CountAsync();

        var result = new PagedQueryResult<TSource>(totalItems, request.Page, request.PageSize);

        result.Items = await source
            .OrderBy(request.OrderBy)
            .Skip(request.SkipCount())
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return result;
    }
}