namespace Honamic.Framework.Queries.GridColumns;

[AttributeUsage(AttributeTargets.Property)]
public class GridColumnAttribute : Attribute
{
    public string? Title { get; set; }
    public int Order { get; set; } = 0;
    public bool Visible { get; set; } = true;
    public bool Sortable { get; set; } = true;
    public bool Filterable { get; set; } = true;
    public bool GenerateField { get; set; } = true;
}