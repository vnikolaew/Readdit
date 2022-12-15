using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Readdit.Common;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Seeding;

public class UserRolesSeeder : ISeeder
{
    public int? Priority => 10;

    public async Task SeedAsync(ReadditDbContext context, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        await SeedRoleAsync(userManager, GlobalConstants.AdministratorRoleName, GlobalConstants.AdminEmail);
    }
    
    private static async Task SeedRoleAsync(
        UserManager<ApplicationUser> userManager,
        string roleName,
        string userEmail)
    {
        var users = userManager
            .Users
            .Where(u => u.Email.Contains(userEmail))
            .ToList();

        foreach (var user in users.Where(user => !user.Roles.Any()))
        {
            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}