using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class PostVoteConfiguration : IEntityTypeConfiguration<PostVote>
{
    public void Configure(EntityTypeBuilder<PostVote> builder)
    {
        builder
            .HasOne(pv => pv.Post)
            .WithMany(p => p.Votes)
            .HasForeignKey(pv => pv.PostId);

        builder
            .HasOne(pv => pv.User)
            .WithMany()
            .HasForeignKey(pv => pv.UserId);
    }
}