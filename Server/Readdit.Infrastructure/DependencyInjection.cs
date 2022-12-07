using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Data;
using Readdit.Infrastructure.Data.Repositories;
using Readdit.Infrastructure.Data.Seeding;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContext<ReadditDbContext>(options =>
            {
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(new AuditInfoSaveChangesInterceptor());
            })
            .AddScoped<ReadditDbContext>();

        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ReadditDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddSeeding()
            .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
        
        return services;
    }

    private static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        var seederTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ISeeder).IsAssignableFrom(t)
                        && t is { IsAbstract: false, IsInterface: false });
        foreach (var seederType in seederTypes)
        {
            services.AddScoped(typeof(ISeeder), seederType);
        }

        services.AddScoped<ReadditDbContextSeeder>();
        return services;
    }

    public static async Task SeedAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var seeder = scope.ServiceProvider.GetRequiredService<ReadditDbContextSeeder>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReadditDbContext>();

        await seeder.SeedAsync(dbContext, scope.ServiceProvider);
    }
}