using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Data.Extensions;
using Readdit.Infrastructure.Models;

namespace Readdit.Infrastructure.Data;

public class ReadditDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
        typeof(ReadditDbContext)
            .GetMethod(nameof(SetIsDeletedFilter),
                BindingFlags.NonPublic | BindingFlags.Static)!;
    
    public ReadditDbContext(DbContextOptions<ReadditDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<Community> Communities { get; set; }

    public DbSet<CommunityPost> CommunityPosts { get; set; }

    public DbSet<CommunityTag> CommunityTags { get; set; }

    public DbSet<PostComment> PostComments { get; set; }

    public DbSet<PostTag> PostTags { get; set; }

    public DbSet<PostVote> PostVotes { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<CommentVote> CommentVotes { get; set; }
    
    public DbSet<UserCommunity> UserCommunities { get; set; }
    
    public DbSet<UserScore> UserScores { get; set; }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        EntityIndicesConfiguration.Configure(builder);
        
        var entityTypes = builder.Model.GetEntityTypes().ToList();

        var deletableTypes = builder.GetEntityTypes<IDeletableEntity>();
        foreach (var deletableType in deletableTypes)
        {
            var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableType);
            method.Invoke(null, new object[] { builder });
        }

        var foreignKeys = entityTypes
            .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
        foreach (var foreignKey in foreignKeys)
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        base.OnModelCreating(builder);
    }

    private static void SetIsDeletedFilter<T>(ModelBuilder builder)
        where T : class, IDeletableEntity
        => builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
}