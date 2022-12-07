using Microsoft.EntityFrameworkCore;

namespace Readdit.Infrastructure.Data.Seeding;

public interface ISeeder
{
    Task SeedAsync(ReadditDbContext context, IServiceProvider serviceProvider);
}