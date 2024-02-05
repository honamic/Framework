using Honamic.Framework.Queries;

namespace Honamic.IdentityPlus.Application.Users.Queries;

public class GetAllUsersQueryFilter : PagedQueryFilter, IQueryFilter
{
    public GetAllUsersQueryFilter()
    {
        OrderBy = "Id desc";
    }
}
