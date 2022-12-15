using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Readdit.Common;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Seeding;

public class RolesSeeder : ISeeder
{
    public int? Priority => 1;

    public async Task SeedAsync(ReadditDbContext context, IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        
        await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
        await SeedRoleAsync(roleManager, GlobalConstants.RegularUserRoleName);
    }
    
    private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            var result = await roleManager.CreateAsync(new ApplicationRole{ Name = roleName });
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }
    }
}