using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class UserCommunityConfiguration : IEntityTypeConfiguration<UserCommunity>
{
    public void Configure(EntityTypeBuilder<UserCommunity> builder)
    {
        builder.HasKey(uc => new { uc.CommunityId, uc.UserId });
    }
}