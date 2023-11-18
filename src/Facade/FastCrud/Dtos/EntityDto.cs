using AutoMapper;
using Honamic.Framework.Facade.FastCrud.Mapping;

namespace Honamic.Framework.Facade.FastCrud.Dtos
{
    public abstract class EntityDto<TEntityDto, TEntity, TPrimaryKey> : EntityDto<TPrimaryKey>, IHaveFastCrudMapping
    {

        #region  Mapping

        public TEntity ToEntity(IMapper mapper)
        {
            return mapper.Map<TEntity>(CastToDerivedClass(mapper, this));
        }

        public TEntity ToEntity(IMapper mapper, TEntity entity)
        {
            return mapper.Map(CastToDerivedClass(mapper, this), entity);
        }

        public static TEntityDto FromEntity(IMapper mapper, TEntity model)
        {
            return mapper.Map<TEntityDto>(model);
        }

        protected TEntityDto CastToDerivedClass(IMapper mapper, EntityDto<TEntityDto, TEntity, TPrimaryKey> baseInstance)
        {
            return mapper.Map<TEntityDto>(baseInstance);
        }

        #endregion

        #region IHaveCustomMapping

        public void CreateMappings(Profile profile)
        {
            var mappingExpression = profile.CreateMap<TEntityDto, TEntity>();

            var dtoType = typeof(TEntityDto);
            var entityType = typeof(TEntity);
            //Ignore any property of source (like Post.Author) that dose not contains in destination 
            foreach (var property in entityType.GetProperties())
            {
                if (dtoType.GetProperty(property.Name) == null)
                    mappingExpression.ForMember(property.Name, opt => opt.Ignore());
            }

            CustomToEntityMappings(mappingExpression);

            CustomFromEntityMappings(mappingExpression.ReverseMap());
        }

        public virtual void CustomFromEntityMappings(IMappingExpression<TEntity, TEntityDto> mapping)
        {

        }

        public virtual void CustomToEntityMappings(IMappingExpression<TEntityDto, TEntity> mapping)
        {

        }

        #endregion

    }

    public abstract class EntityDto<TEntityDto, TEntity> : EntityDto<TEntityDto, TEntity, long>
    {

    }

    public class EntityDto : EntityDto<long>, IEntityDto<long>
    {

    }

    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
    }
}