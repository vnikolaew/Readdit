using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(u => u.Communities)
            .WithOne(uc => uc.User)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.CommunitiesAdministrated)
            .WithOne(c => c.Admin)
            .HasForeignKey(c => c.AdminId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(u => u.CommunityPosts)
            .WithOne(p => p.Author)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}