namespace Honamic.Framework.Application.Authorizes; 

/// <summary>
/// Supplies the selectable values for a permission scope (e.g. the list of roles for a "Role" scope),
/// enabling a dynamic value picker in the role-permissions editor.
/// Implementations are resolved by <see cref="Key"/>, which must match the scope's ValueSourceKey.
/// </summary>
public interface IScopeValueProvider
{
    /// <summary>
    /// The value-source key this provider serves. Must equal the scope's ValueSourceKey.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Searches selectable values (with paging) for the picker.
    /// </summary>
    Task<ScopeValuePage> SearchAsync(ScopeValueQuery query, CancellationToken cancellationToken);

    /// <summary>
    /// Resolves display values for already-stored raw values (reverse lookup when opening the editor).
    /// </summary>
    Task<List<ScopeValueItem>> ResolveAsync(IReadOnlyCollection<string> values, CancellationToken cancellationToken);
}


public class ScopeValueItem
{
    public string Value { get; set; } = default!;

    public string DisplayValue { get; set; } = default!;
}

public class ScopeValueQuery
{
    public string? Search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}

public class ScopeValuePage
{
    public List<ScopeValueItem> Items { get; set; } = new();

    public int Total { get; set; }
}
