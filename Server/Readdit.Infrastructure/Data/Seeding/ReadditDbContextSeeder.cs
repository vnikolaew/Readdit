using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Readdit.Infrastructure.Data.Seeding;

public class ReadditDbContextSeeder : ISeeder
{
    public async Task SeedAsync(ReadditDbContext context, IServiceProvider serviceProvider)
    {
        if (context is null || serviceProvider is null)
        {
            return;
        }

        var logger = serviceProvider.GetRequiredService<ILogger<ReadditDbContextSeeder>>();
        var seeders = serviceProvider
            .GetServices<ISeeder>()
            .Where(s => s is not ReadditDbContextSeeder)
            .ToList();
        
        logger.LogInformation("Starting database seeding ... Using {0}.",
            string.Join(", ", seeders.Select(s => s.GetType().Name)));
        
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(context, serviceProvider);
        }
        logger.LogInformation("Database seeding done.");
    }
}