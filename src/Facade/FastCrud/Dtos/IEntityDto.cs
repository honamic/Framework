namespace Honamic.Framework.Facade.FastCrud.Dtos
{
    public interface IEntityDto<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }

    public interface IEntityDto : IEntityDto<long>
    {

    }
}
