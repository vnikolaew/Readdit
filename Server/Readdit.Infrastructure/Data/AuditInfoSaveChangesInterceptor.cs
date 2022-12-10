using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Data;

public class AuditInfoSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var changedEntries = eventData
            .Context
            .ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(e => e.State is EntityState.Added
                or EntityState.Modified);

        foreach (var entry in changedEntries)
        {
            var entity = entry.Entity;
            
            if(entry.State == EntityState.Added && entity.CreatedOn == default)
            {
                entity.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
            }
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}