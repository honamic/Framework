using System.Linq.Expressions;

namespace Honamic.Framework.EntityFramework.Extensions;
public static class LinqExtentions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// Splits the keyword by spaces and applies AND between parts, OR between selectors.
    /// Example: "brake pad" → (Name LIKE '%brake%' OR Code LIKE '%brake%') AND (Name LIKE '%pad%' OR Code LIKE '%pad%')
    /// </summary>
    public static IQueryable<T> WhereContainsWords<T>(this IQueryable<T> source, string? keyword, params Expression<Func<T, string?>>[] selectors)
        => source.WhereContainsWords(keyword, 5, selectors);

    public static IQueryable<T> WhereContainsWords<T>(this IQueryable<T> source, string? keyword, int maxParts, params Expression<Func<T, string?>>[] selectors)
    {
        if (string.IsNullOrWhiteSpace(keyword) || selectors.Length == 0)
            return source;

        var parts = keyword.Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(maxParts)
            .ToArray();

        var parameter = selectors[0].Parameters[0];
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;

        foreach (var part in parts)
        {
            Expression? partCondition = null;

            foreach (var selector in selectors)
            {
                var selectorBody = new ParameterReplacer(selector.Parameters[0], parameter).Visit(selector.Body);
                var callExpr = Expression.Call(selectorBody, containsMethod, Expression.Constant(part));
                partCondition = partCondition is null ? callExpr : Expression.OrElse(partCondition, callExpr);
            }

            source = source.Where(Expression.Lambda<Func<T, bool>>(partCondition!, parameter));
        }

        return source;
    }

    /// <summary>
    /// Applies a single Contains check across all selectors with OR, skipping null or empty keywords.
    /// Example: "brake" → Name LIKE '%brake%' OR Code LIKE '%brake%'
    /// </summary>
    public static IQueryable<T> WhereContains<T>(this IQueryable<T> source, string? keyword, params Expression<Func<T, string?>>[] selectors)
    {
        if (string.IsNullOrWhiteSpace(keyword) || selectors.Length == 0)
            return source;

        var parameter = selectors[0].Parameters[0];
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;

        Expression? condition = null;

        foreach (var selector in selectors)
        {
            var selectorBody = new ParameterReplacer(selector.Parameters[0], parameter).Visit(selector.Body);
            var callExpr = Expression.Call(selectorBody, containsMethod, Expression.Constant(keyword.Trim()));
            condition = condition is null ? callExpr : Expression.OrElse(condition, callExpr);
        }

        return source.Where(Expression.Lambda<Func<T, bool>>(condition!, parameter));
    }

    private sealed class ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
            => node == oldParam ? newParam : base.VisitParameter(node);
    }
}
