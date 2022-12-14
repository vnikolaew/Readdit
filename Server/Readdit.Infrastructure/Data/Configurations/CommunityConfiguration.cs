using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class CommunityConfiguration : IEntityTypeConfiguration<Community>
{
    public void Configure(EntityTypeBuilder<Community> builder)
    {
        builder
            .HasIndex(c => c.Name)
            .IsUnique();
            
        builder
            .HasOne(c => c.Admin)
            .WithMany(u => u.CommunitiesAdministrated)
            .HasForeignKey(c => c.AdminId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(c => c.Tags)
            .WithOne(t => t.Community)
            .HasForeignKey(ct => ct.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(c => c.Posts)
            .WithOne(p => p.Community)
            .HasForeignKey(p => p.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.Members)
            .WithOne(uc => uc.Community)
            .HasForeignKey(uc => uc.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}