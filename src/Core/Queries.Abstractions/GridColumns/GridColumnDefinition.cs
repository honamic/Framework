using System.Linq.Expressions;
using System.Reflection;

namespace Honamic.Framework.Queries.GridColumns;

public class GridColumnDefinition<T>
{
    public string Title { get; set; } = default!;

    public bool Visible { get; set; } = true;

    public bool Sortable { get; set; } = true;

    public bool Filterable { get; set; } = true;

    public bool GenerateField { get; set; } = true;

    public int Order { get; set; } = int.MaxValue;

    public PropertyInfo Property { get; set; } = default!;

    public LambdaExpression PropertyExpression { get; set; } = default!;
}