using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasOne(u => u.Country)
            .WithMany();

        builder
            .OwnsOne(u => u.Profile, p =>
            {
                p.WithOwner(p => p.User)
                    .HasForeignKey(pr => pr.UserId);
                p.ToTable($"{nameof(UserProfile)}s");
                p.HasKey(p => p.Id);
            });
        
        builder
            .HasMany(u => u.Communities)
            .WithOne(uc => uc.User)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.CommunityPosts)
            .WithOne(p => p.Author)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}