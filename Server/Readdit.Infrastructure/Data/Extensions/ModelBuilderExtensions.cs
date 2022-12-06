using Microsoft.EntityFrameworkCore;

namespace Readdit.Infrastructure.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static IEnumerable<Type> GetEntityTypes<TEntity>(this ModelBuilder builder)
        where TEntity : class
        => builder
            .Model
            .GetEntityTypes()
            .Where(et => typeof(TEntity).IsAssignableFrom(et.ClrType))
            .Select(et => et.ClrType);
}