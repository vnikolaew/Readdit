using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Readdit.Common;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Seeding;

public class UsersSeeder : ISeeder
{
    public int? Priority => 1;

    public async Task SeedAsync(ReadditDbContext context, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedUserAsync(
            userManager,
            GlobalConstants.AdminEmail,
            GlobalConstants.AdminName,
            GlobalConstants.AdminName);
    }
    
    private static async Task SeedUserAsync(
        UserManager<ApplicationUser> userManager,
        string userEmail,
        string firstName,
        string lastName)
    {
        var user = await userManager
            .FindByEmailAsync(userEmail);

        if (user == null)
        {
            var admin = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = firstName,
                Email = userEmail,
                EmailConfirmed = true,
                PasswordHash = GlobalConstants.SystemPasswordHashed,
                Profile = new UserProfile
                {
                    ProfilePictureUrl = GlobalConstants.AdminPicture,
                    ProfilePicturePublicId = GlobalConstants.AdminPicturePublicId
                },
                Score = new UserScore()
            };

            await userManager.CreateAsync(admin);
        }
    }
}