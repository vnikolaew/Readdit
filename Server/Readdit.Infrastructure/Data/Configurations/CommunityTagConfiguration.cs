using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class CommunityTagConfiguration : IEntityTypeConfiguration<CommunityTag>
{
    public void Configure(EntityTypeBuilder<CommunityTag> builder)
    {
        builder.HasKey(ct => new { ct.CommunityId, ct.TagId });

        builder
            .HasOne(ct => ct.Tag)
            .WithMany()
            .HasForeignKey(ct => ct.TagId);

        builder
            .HasOne(ct => ct.Community)
            .WithMany(c => c.Tags)
            .HasForeignKey(ct => ct.CommunityId);
    }
}