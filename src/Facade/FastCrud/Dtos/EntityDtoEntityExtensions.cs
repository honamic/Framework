using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Honamic.Framework.Facade.FastCrud.Dtos
{
    public static class EntityDtoEntityExtensions
    {
        public static TEntity ToEntity<TEntity, TViewModel>(this IMapper mapper, TViewModel viewModel)
        {
            return mapper.Map<TEntity>(viewModel);
        }

        //public TEntity ToEntity<TViewModel, TEntity>(this IMapper mapper, TEntity entity)
        //{
        //    return mapper.Map(CastToDerivedClass<TViewModel, TEntity>(mapper, this), entity);
        //}

        public static TViewModel FromEntitythis<TViewModel, TEntity>(this IMapper mapper, TEntity model)
        {
            return mapper.Map<TViewModel>(model);
        }

        //protected static TViewModel CastToDerivedClass<TViewModel, TEntity>(this IMapper mapper, BaseViewModel<TViewModel, TEntity, TKey> baseInstance)
        //{
        //    return mapper.Map<TViewModel>(baseInstance);
        //}
    }
}
