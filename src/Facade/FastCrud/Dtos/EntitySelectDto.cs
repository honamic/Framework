using Honamic.Framework.Utilities.Extensions;
using System.Text.Json.Serialization;

namespace Honamic.Framework.Facade.FastCrud.Dtos;

public abstract class EntitySelectDto<TEntityDto, TEntity> : EntitySelectDto<TEntityDto, TEntity, long>
{

}

public abstract class EntitySelectDto<TEntityDto, TEntity, TPrimaryKey> : EntityDto<TEntityDto, TEntity, TPrimaryKey>
{
    [JsonIgnore]
    public virtual string Name { get; set; }

    [JsonIgnore]
    public virtual string Title { get; set; }

    public virtual string Text => ToString();

    public override string ToString()
    {
        return Title.HasValue() ? Title : Name;
    }
}