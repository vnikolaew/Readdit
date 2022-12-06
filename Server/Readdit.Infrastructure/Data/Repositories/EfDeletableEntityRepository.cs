﻿using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Common.Repositories;

namespace Readdit.Infrastructure.Data.Repositories;

public class EfDeletableEntityRepository<TEntity>
    : EfRepository<TEntity>, IDeletableEntityRepository<TEntity>
    where TEntity : class, IDeletableEntity
{
    public EfDeletableEntityRepository(ReadditDbContext context)
        : base(context)
    {
    }

    public override IQueryable<TEntity> All()
        => base.All().Where(x => !x.IsDeleted);

    public override IQueryable<TEntity> AllAsNoTracking()
        => base.AllAsNoTracking().Where(x => !x.IsDeleted);

    public IQueryable<TEntity> AllWithDeleted()
        => base.All().IgnoreQueryFilters();

    public IQueryable<TEntity> AllAsNoTrackingWithDeleted()
        => base.AllAsNoTracking().IgnoreQueryFilters();

    public Task<TEntity> GetByIdWithDeletedAsync(params object[] id)
    {
        var getByIdPredicate = EfExpressionHelper.BuildByIdPredicate<TEntity>(Context, id);
        return AllWithDeleted()
            .FirstOrDefaultAsync(getByIdPredicate)!;
    }

    public void HardDelete(TEntity entity)
        => base.Delete(entity);

    public void Undelete(TEntity entity)
    {
        entity.IsDeleted = false;
        entity.DeletedOn = null;
        Update(entity);
    }

    public override void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedOn = DateTime.UtcNow;
        Update(entity);
    } 
}