using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Applications.Results;
using Honamic.Framework.Queries;

namespace Honamic.Todo.Query.Domain.TodoItems.Queries;

[DynamicPermission(
    DisplayName = "مدیریت کاربران | کاربران | لیست",
    Group = "Users",
    Module = "UserManagement",
    Name = null,
    Description = null)]
[ScopeDynamicPermission("User", "کاربر")]
[ScopeDynamicPermission("Category", "دسته‌بندی")]
[FieldDynamicPermission("Date", "تاریخ")]
[FieldDynamicPermission("Name", "نام")]

public class GetAllTodoItemsQueryFilter : PagedQueryFilter, IQuery<Result<PagedQueryResult<GetAllTodoItemsQueryResult>>>
{
    public GetAllTodoItemsQueryFilter()
    {
        OrderBy = "Id desc";
    }
}
