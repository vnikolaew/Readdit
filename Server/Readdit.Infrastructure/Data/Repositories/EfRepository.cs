﻿using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;

namespace Readdit.Infrastructure.Data.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    public EfRepository(ReadditDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = Context.Set<TEntity>();
    }

    protected DbSet<TEntity> DbSet { get; set; }

    protected ReadditDbContext Context { get; set; }

    public virtual IQueryable<TEntity> All() => DbSet;

    public virtual IQueryable<TEntity> AllAsNoTracking()
        => DbSet.AsNoTracking();

    public virtual void Add(TEntity entity)
        => DbSet.Add(entity);

    public virtual void Update(TEntity entity)
    {
        var entry = Context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        entry.State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
        => DbSet.Remove(entity);

    public Task<int> SaveChangesAsync()
        => Context.SaveChangesAsync();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Context.Dispose();
        }
    } 
}