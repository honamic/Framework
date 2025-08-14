namespace Honamic.Framework.Queries;

public class PagedQueryResult<TEntity>
{
    public PagedQueryResult()
    {
        Items = new List<TEntity>();
    }

    public PagedQueryResult(int totalItems, int pageNumber, int pageSize)
    {
        TotalItems = totalItems;
        Page = pageNumber;
        PageSize = pageSize;
        Items = new List<TEntity>();
    }

    public List<TEntity> Items { get; set; }

    public int TotalItems { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}