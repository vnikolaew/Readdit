using Readdit.Infrastructure.Data;

namespace Readdit.Services.Data.Common.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ReadditDbContext _dbContext;

    public UnitOfWork(ReadditDbContext dbContext)
        => _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}