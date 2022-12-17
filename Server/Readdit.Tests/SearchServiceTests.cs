using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.PostFeed.Enums;
using Readdit.Services.Data.Search;
using Readdit.Services.Data.Search.Models;
using Readdit.Services.Mapping;
using Readdit.Tests.Common;

namespace Readdit.Tests;

[TestFixture]
public class SearchServiceTests
{
    private static SearchService _searchService;
    private static IRepository<Community> _communityRepo;
    private static IRepository<CommunityPost> _communityPostRepo;
    private static IRepository<ApplicationUser> _usersRepo;

    private static readonly ApplicationUser AdminUser = new()
    {
        UserName = "AdminUser",
        Id = "AdminId",
        FirstName = "Admin",
        LastName = "User"
    };
    
    private static readonly Community TestCommunity = new()
    {
        AdminId = AdminUser.Id,
        Admin = AdminUser,
        Type = CommunityType.Public,
        Id = "CommunityId",
        Name = "TestCommunity", 
        Description = "Some description",
        PictureUrl = "Some Picture Url",
        PicturePublicId = "Some Picture Id"
    };

    private const int TotalCommunities = 10;

    private const string QueryString = "abc";

    private static IEnumerable<Community> GetCommunitiesWithMatchingName
        => Enumerable.Range(1, 10).Select(i => new Community
        {
            Admin = AdminUser,
            Type = CommunityType.Public,
            Name = $"Community {i} {QueryString}",
            Description = $"Community {i} description",
            PictureUrl = $"Community {i} Picture Url",
            PicturePublicId = $"Community {i} Picture Public Id",
        });
    
    private static IEnumerable<Community> GetCommunitiesWithMatchingDescription
        => Enumerable.Range(1, 10).Select(i => new Community
        {
            Admin = AdminUser,
            Type = CommunityType.Public,
            Name = $"Community {i}",
            Description = $"Community {i} description {QueryString}",
            PictureUrl = $"Community {i} Picture Url",
            PicturePublicId = $"Community {i} Picture Public Id",
        });
    
    private static IEnumerable<CommunityPost> GetPostsWithMatchingTitle
        => Enumerable.Range(1, 10).Select(i => new CommunityPost
        {
            Author = AdminUser,
            CreatedOn = DateTime.Now.AddDays(-(10 - i)),
            Title = $"Title {i} {QueryString}",
            Content = $"Post {i} content",
            MediaUrl = $"Post {i} Media Url",
            MediaPublicId = $"Post {i} Media Public Id",
            Community = TestCommunity
        });
    
    private static IEnumerable<ApplicationUser> GetUsersWithMatchingUsernames
        => Enumerable.Range(1, 10).Select(i => new ApplicationUser
        {
            CreatedOn = DateTime.Now.AddDays(-(10 - i)),
            UserName = $"Username {i} {QueryString}",
            FirstName = $"First Name {i}",
            LastName = $"First Name {i}",
            Email = $"test@test.com",
            Profile = new UserProfile()
            {
                ProfilePicturePublicId = "",
                ProfilePictureUrl = "",
                CountryId = ""
            },
            Score = new UserScore()
        });

    [OneTimeSetUp]
    public void SetUp()
    {
        MappingConfiguration.RegisterMappings(typeof(MappingConfiguration).Assembly);
        _communityRepo = DbRepositoryProvider.Get<Community>();
        _communityPostRepo = DbRepositoryProvider.Get<CommunityPost>();
        _usersRepo = DbRepositoryProvider.Get<ApplicationUser>();
        
        _searchService = new SearchService(
            _communityRepo, _communityPostRepo, _usersRepo);
        
        var context = InMemoryDbContextProvider.Instance;
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        var context = InMemoryDbContextProvider.Instance;
        context.Database.EnsureDeleted();
    }

    [Test]
    public async Task SearchCommunitiesWithMatchingName_ShouldWorkAsExpected()
    {
        foreach (var community in GetCommunitiesWithMatchingName)
        {
            _communityRepo.Add(community);
        }

        await _communityRepo.SaveChangesAsync();
        var results = await _searchService.SearchCommunities<CommunitySearchModel>(QueryString);
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count(), Is.EqualTo(TotalCommunities));
    }
    
    [Test]
    public async Task SearchCommunitiesWithMatchingDescription_ShouldWorkAsExpected()
    {
        foreach (var community in GetCommunitiesWithMatchingDescription)
        {
            _communityRepo.Add(community);
        }

        await _communityRepo.SaveChangesAsync();
        var results = await _searchService.SearchCommunities<CommunitySearchModel>(QueryString);
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count(), Is.EqualTo(TotalCommunities));
    }
    
    [Test]
    public async Task SearchPostsWithMatchingTitle_ShouldWorkAsExpected()
    {
        _usersRepo.Add(AdminUser);
        await _usersRepo.SaveChangesAsync();
        
        _communityRepo.Add(TestCommunity);
        await _communityRepo.SaveChangesAsync();
        
        foreach (var post in GetPostsWithMatchingTitle)
        {
            _communityPostRepo.Add(post);
        }

        await _communityPostRepo.SaveChangesAsync();
        
        var results = await _searchService.SearchPosts<PostSearchModel>(QueryString, TimeRange.ThisWeek);
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count(), Is.EqualTo(8));
    }
    
    [Test]
    public async Task SearchUsersWithMatchingUsernames_ShouldWorkAsExpected()
    {
        foreach (var user in GetUsersWithMatchingUsernames)
        {
            _usersRepo.Add(user);
        }

        await _usersRepo.SaveChangesAsync();
        
        var results = await _searchService.SearchUsers<UserSearchModel>(QueryString);
        
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count(), Is.EqualTo(10));
    }
}