using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Data;

public static class EntityIndicesConfiguration
{
    public static void Configure(ModelBuilder builder)
    {
        var deletableEntityTypes = builder
            .Model
            .GetEntityTypes()
            .Where(et => typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
        
        foreach (var deletableEntityType in deletableEntityTypes)
        {
            builder
                .Entity(deletableEntityType.ClrType)
                .HasIndex(nameof(IDeletableEntity.IsDeleted));
        }
    }
    
}