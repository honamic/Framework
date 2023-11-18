using AutoMapper;

namespace Honamic.Framework.Facade.FastCrud.Mapping;

public class FastCrudMappingProfile : Profile
{
    public FastCrudMappingProfile(IEnumerable<IHaveFastCrudMapping> haveCustomMappings)
    {
        foreach (var item in haveCustomMappings)
            item.CreateMappings(this);
    }
}
