using System.Linq.Expressions;
using System.Reflection;

namespace Honamic.Framework.Queries.GridColumns;

public static class GridColumnExtensions
{
    public static List<GridColumnDefinition<T>> GetGridColumns<T>()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
             .Where(p => p.CanRead)
             .Select(p =>
             {
                 var attr = p.GetCustomAttribute<GridColumnAttribute>();
                 return new GridColumnDefinition<T>
                 {
                     Title = attr?.Title ?? p.Name.SeparateCamelCase()!,
                     Visible = attr?.Visible ?? true,
                     Sortable = attr?.Sortable ?? true,
                     Filterable = attr?.Filterable ?? true,
                     Order = attr?.Order ?? int.MaxValue,
                     GenerateField = attr?.GenerateField ?? true,
                     Property = p,
                     PropertyExpression = BuildTypedExpression<T>(p)
                 };
             })
             .Where(x => x.GenerateField)
             .OrderBy(x => x.Order)
             .ToList();
    }
 
    public static LambdaExpression BuildTypedExpression<T>(PropertyInfo prop)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var body = Expression.Property(parameter, prop);
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), prop.PropertyType);
        return Expression.Lambda(delegateType, body, parameter);
    }

    private static string? SeparateCamelCase(this string? stringValue)
    {
        if (stringValue is null)
            return stringValue;

        for (int i = 1; i < stringValue.Length; i++)
        {
            if (char.IsUpper(stringValue[i]))
            {
                stringValue = stringValue.Insert(i, " ");
                i++;
            }
        }

        return stringValue;
    }
}