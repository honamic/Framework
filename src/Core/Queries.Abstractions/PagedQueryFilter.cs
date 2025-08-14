using System.ComponentModel.DataAnnotations;

namespace Honamic.Framework.Queries;

public class PagedQueryFilter : QueryFilter
{
    public PagedQueryFilter()
    {
        Page = 1;
        PageSize = 10;
    }

    public PagedQueryFilter(int pageNumber, int pageSize)
    {
        Page = pageNumber;
        PageSize = pageSize;
    }

    [Range(1, int.MaxValue)]
    public int Page { get; set; }

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }

    public int SkipCount() => Page * PageSize - PageSize;
}