using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Data.Repositories;

namespace Readdit.Tests.Common;

public static class DbRepositoryProvider
{
    public static IRepository<TEntity> Get<TEntity>()
        where TEntity : class
        => new EfRepository<TEntity>(InMemoryDbContextProvider.Instance);
    
    public static IDeletableEntityRepository<TEntity> GetDeletable<TEntity>()
        where TEntity : class, IDeletableEntity
        => new EfDeletableEntityRepository<TEntity>(InMemoryDbContextProvider.Instance);
}