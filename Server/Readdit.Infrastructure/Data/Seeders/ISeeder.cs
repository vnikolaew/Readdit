using Microsoft.EntityFrameworkCore;

namespace Readdit.Infrastructure.Data.Seeders;

public interface ISeeder
{
    Task SeedAsync(DbContext context, IServiceProvider serviceProvider);
}