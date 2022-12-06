using System.Reflection;
using AutoMapper;

namespace Readdit.Services.Mapping;

public static class MappingConfiguration
{
    private static bool _initialized;

    public static IMapper? MapperInstance { get; set; }

    public static void RegisterMappings(params Assembly[] assemblies)
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

        var config = new MapperConfigurationExpression();
        config.CreateProfile(
            "DefaultProfile",
            configuration =>
            {
                // IMapFrom<>
                foreach (var map in GetFromMaps(types))
                {
                    configuration.CreateMap(map.Source, map.Destination);
                }

                // IMapTo<>
                foreach (var map in GetToMaps(types))
                {
                    configuration.CreateMap(map.Source, map.Destination);
                }

                // IHaveCustomMappings
                foreach (var map in GetCustomMappings(types))
                {
                    map.CreateMappings(configuration);
                }
            });
        MapperInstance = new Mapper(new MapperConfiguration(config));
    }

    private static IEnumerable<TypesMap> GetFromMaps(IEnumerable<Type> types)
        => types
            .SelectMany(t => t.GetTypeInfo().GetInterfaces())
            .Where(t => t.GetTypeInfo().IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(IMapFrom<>)
                        && !t.GetTypeInfo().IsAbstract
                        && !t.GetTypeInfo().IsInterface)
            .Select(t => new TypesMap()
            {
                Source = t.GetTypeInfo().GetGenericArguments()[0],
                Destination = t
            });

    private static IEnumerable<TypesMap> GetToMaps(IEnumerable<Type> types)
        => types
            .SelectMany(t => t.GetTypeInfo().GetInterfaces())
            .Where(t => t.GetTypeInfo().IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(IMapTo<>)
                        && !t.GetTypeInfo().IsAbstract
                        && !t.GetTypeInfo().IsInterface)
            .Select(t => new TypesMap()
            {
                Source = t,
                Destination = t.GetTypeInfo().GetGenericArguments()[0]
            });

    private static IEnumerable<IHaveCustomMappings> GetCustomMappings(IEnumerable<Type> types)
        => types
            .SelectMany(t => t.GetTypeInfo().GetInterfaces())
            .Where(t => typeof(IHaveCustomMappings).GetTypeInfo().IsAssignableFrom(t)
                        && !t.GetTypeInfo().IsAbstract
                        && !t.GetTypeInfo().IsInterface)
            .Select(t => (IHaveCustomMappings)Activator.CreateInstance(t)!);

    private class TypesMap
    {
        public Type Source { get; set; }

        public Type Destination { get; set; }
    }
}