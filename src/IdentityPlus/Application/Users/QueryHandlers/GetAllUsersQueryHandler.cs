using Honamic.Framework.Queries;
using Honamic.IdentityPlus.Application.Users.Queries;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Honamic.Framework.Utilities.Extensions;

namespace Honamic.IdentityPlus.Application.Users.QueryHandlers;
public class GetAllUsersQueryHandler(DbContext dbContext)
    : IQueryHandler<GetAllUsersQueryFilter, PagedQueryResult<GetAllUsersQueryResult>>
{

    public Task<PagedQueryResult<GetAllUsersQueryResult>>
        HandleAsync(GetAllUsersQueryFilter filter, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<User>().AsQueryable();

        if (filter.Keyword.HasValue())
        {
            query = query.Where(c =>
                 c.UserName.Contains(filter.Keyword)
                 || c.Email.Contains(filter.Keyword)
                 || c.PhoneNumber.Contains(filter.Keyword)
            );
        }

        return query.Select(x => new GetAllUsersQueryResult()
        {
            Id = x.Id,
            Username = x.UserName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
        }).ToPagedListAsync(filter, cancellationToken);
    }
}