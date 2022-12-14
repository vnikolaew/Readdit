namespace Readdit.Infrastructure.Common.Repositories;

public interface IRepository<TEntity> : IDisposable
    where TEntity : class
{
    IQueryable<TEntity> All();

    IQueryable<TEntity> AllAsNoTracking();

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    Task<int> SaveChangesAsync(); 
}