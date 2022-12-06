using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data.Configurations;

public class CommentVoteConfiguration : IEntityTypeConfiguration<CommentVote>
{
    public void Configure(EntityTypeBuilder<CommentVote> builder)
    {
        builder
            .HasOne(cv => cv.Comment)
            .WithMany(c => c.Votes)
            .HasForeignKey(cv => cv.CommentId);

        builder
            .HasOne(cv => cv.User)
            .WithMany()
            .HasForeignKey(cv => cv.UserId);
    }
}