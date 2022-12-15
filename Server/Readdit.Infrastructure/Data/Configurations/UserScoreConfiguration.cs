using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class UserScoreConfiguration : IEntityTypeConfiguration<UserScore>
{
    public void Configure(EntityTypeBuilder<UserScore> builder)
    {
        builder
            .HasOne(us => us.User)
            .WithOne(u => u.Score)
            .HasForeignKey<UserScore>(us => us.Id);

        builder
            .Property(us => us.PostsScore)
            .HasDefaultValue(0);
        
        builder
            .Property(us => us.CommentsScore)
            .HasDefaultValue(0);
    }
}