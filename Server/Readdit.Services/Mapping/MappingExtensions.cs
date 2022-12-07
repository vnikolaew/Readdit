using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;

namespace Readdit.Services.Mapping;

public static class MappingExtensions
{
    public static IQueryable<TDestination> To<TDestination>(
        this IQueryable source,
        params Expression<Func<TDestination, object>>[] membersToExpand)
    {
        return source is null
            ? throw new ArgumentNullException(nameof(source))
            : source.ProjectTo(MappingConfiguration.MapperInstance!.ConfigurationProvider, null, membersToExpand);
    }
    
    public static IQueryable<TDestination> To<TDestination>(
        this IQueryable source,
        object parameters)
    {
        return source is null
            ? throw new ArgumentNullException(nameof(source))
            : source.ProjectTo<TDestination>(MappingConfiguration.MapperInstance!.ConfigurationProvider,  parameters);
    }

}