namespace Honamic.Framework.Queries;

public abstract class EntityQueryBase<TKey> 
{
    public TKey Id { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }

}