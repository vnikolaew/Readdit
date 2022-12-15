using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder
            .HasOne(p => p.Country)
            .WithMany();

        builder
            .Property(p => p.CountryId)
            .HasDefaultValue("0b8374c5-1387-4729-9ed6-5f34c91dc5a9");
    }
}