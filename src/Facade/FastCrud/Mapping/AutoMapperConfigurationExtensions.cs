using AutoMapper;
using System.Reflection;

namespace Honamic.Framework.Facade.FastCrud.Mapping;

public static class AutoMapperConfigurationExtensions
{
    public static void AddFastCrudMappings(this IMapperConfigurationExpression mapperConfiguration, Assembly[] assemblies)
    {
        var allTypes = assemblies.SelectMany(c => c.ExportedTypes);

        var list = allTypes.Where(type => type.IsClass && !type.IsAbstract &&
            type.GetInterfaces().Contains(typeof(IHaveFastCrudMapping)))
            .Select(type => (IHaveFastCrudMapping)Activator.CreateInstance(type));

        var profile = new FastCrudMappingProfile(list);

        mapperConfiguration.AddProfile(profile);
    }
}
