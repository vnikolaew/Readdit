namespace Readdit.Infrastructure.Data.Seeding;

public interface ISeeder
{
    public int? Priority { get; }
    
    Task SeedAsync(ReadditDbContext context, IServiceProvider serviceProvider);
}