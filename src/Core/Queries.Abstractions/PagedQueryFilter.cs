using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honamic.Framework.Queries;

public abstract class PagedQueryFilter : QueryFilter
{
    protected override abstract string DefaultOrderBy { get; }

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
    [DefaultValue(1)]
    public int Page { get; set; }

    [Range(1, int.MaxValue)]
    [DefaultValue(10)]
    public int PageSize { get; set; }

    public int SkipCount() => Page * PageSize - PageSize;
}