using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Common.Repositories;

public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IDeletableEntity
{
    IQueryable<TEntity> AllWithDeleted();

    IQueryable<TEntity> AllAsNoTrackingWithDeleted();

    Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

    void HardDelete(TEntity entity);

    void Undelete(TEntity entity); 
}