using Honamic.Framework.Queries.GridColumns;
using System.Text.Json.Serialization;

namespace Honamic.Framework.Queries;

public abstract class EntityQueryResult<TKey>
{
    [GridColumn(Order = 0, Visible = false, GenerateField = true)]
    public virtual TKey Id { get; set; } = default!;


    [GridColumn(Order = 1000, Visible = false, GenerateField = true)]
    public virtual string? CreatedBy { get; set; }


    [GridColumn(Order = 1000, Visible = false, GenerateField = true)]
    public virtual string? ModifiedBy { get; set; }


    [GridColumn(GenerateField = false)]
    public virtual DateTimeOffset? CreatedOn { get; set; }

    [GridColumn(Title = nameof(CreatedOn), Order = 1, Visible = false)]
    [JsonIgnore]
    public virtual DateTime? CreatedOnLocal => CreatedOn?.ToOffset(EffectiveOffset()).DateTime;



    [GridColumn(GenerateField = false)]
    public virtual DateTimeOffset? ModifiedOn { get; set; }


    [GridColumn(Title = nameof(ModifiedOn), Order = 1000, Visible = false)]
    [JsonIgnore]
    public virtual DateTime? ModifiedOnLocal => ModifiedOn?.ToOffset(EffectiveOffset()).DateTime;






    [GridColumn(GenerateField = false)]
    [JsonIgnore]
    public static TimeSpan? TimeZoneOffset { get; set; }

    public TimeSpan EffectiveOffset() => TimeZoneOffset ?? TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);


    public override string ToString()
    {
        return $"{Id}";
    }
}