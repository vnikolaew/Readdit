using System.Reflection;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.UserCommunities;
using Readdit.Services.Data.UserCommunities.Models;
using Readdit.Services.Mapping;

namespace Readdit.Tests;

[TestFixture]
public class UserCommunitiesServiceTests
{
    private static UserCommunityService _userCommunityService;
    private IDeletableEntityRepository<UserCommunity> _userCommunities;
    private IRepository<ApplicationUser> _users;
    private IRepository<Community> _communities;

    private static readonly ApplicationUser TestUser = new()
    {
        Id = "TestId",
        FirstName = "John",
        LastName = "Doe"
    }; 
    
    private static readonly ApplicationUser AdminUser = new()
    {
        Id = "AdminId",
        FirstName = "Admin",
        LastName = "User"
    };
    
    private static readonly Community CommunityOne = new()
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
    
    private static readonly Community CommunityTwo = new()
    {
        AdminId = AdminUser.Id,
        Admin = AdminUser,
        Type = CommunityType.Public,
        Id = "CommunityId2",
        Name = "TestCommunity", 
        Description = "Some description",
        PictureUrl = "Some Picture Url",
        PicturePublicId = "Some Picture Id"
    };
        
    [OneTimeSetUp]
    public void SetUp()
    {
        MappingConfiguration.RegisterMappings(typeof(MappingConfiguration).Assembly);
        _users = DbRepositoryProvider.Get<ApplicationUser>();
        _communities = DbRepositoryProvider.Get<Community>();
        _userCommunities = DbRepositoryProvider.GetDeletable<UserCommunity>();

        _userCommunityService = new UserCommunityService(
            _userCommunities, _users, _communities);
        
        var context = InMemoryDbContextProvider.Instance;
        
        _users.Add(TestUser);
        _users.Add(AdminUser);
        
        _communities.Add(CommunityOne);
        _communities.Add(CommunityTwo);

        context.SaveChanges();
    }

    [Test]
    public async Task JoinCommunityAsync_ShouldWorkAsExpected()
    {
        var userCommunity = await _userCommunityService.JoinCommunityAsync(
            TestUser.Id, CommunityOne.Id);
        
        Assert.That(userCommunity!.UserId, Is.EqualTo(TestUser.Id));
        Assert.That(userCommunity.CommunityId, Is.EqualTo(CommunityOne.Id));
        Assert.That(userCommunity.Status, Is.EqualTo(UserCommunityStatus.Approved));
    }
        
    [Test]
    public async Task LeaveCommunityAsync_ShouldWorkAsExpected()
    {
        await _userCommunityService
            .JoinCommunityAsync(TestUser.Id, CommunityOne.Id);
        
        var success = await _userCommunityService.LeaveCommunityAsync(
            TestUser.Id, CommunityOne.Id);
        
        Assert.That(success, Is.EqualTo(true));
    }
    
    [Test]
    public async Task ApproveUserAsync_ShouldWorkAsExpected()
    {
        var context = InMemoryDbContextProvider.Instance;
        var community = new Community
        {
            AdminId = AdminUser.Id,
            Admin = AdminUser,
            Type = CommunityType.Restricted,
            Id = "CommunityId2",
            Name = "TestCommunity2", 
            Description = "Some description2",
            PictureUrl = "Some Picture Url2",
            PicturePublicId = "Some Picture Id2"
        };

        _communities.Add(community);
        await context.SaveChangesAsync();
        
        await _userCommunityService
            .JoinCommunityAsync(TestUser.Id, community.Id);
            
        var userCommunity = await _userCommunityService.ApproveUserAsync(
            AdminUser.Id, TestUser.Id, community.Id);
            
        Assert.That(userCommunity, Is.Not.EqualTo(null));
        Assert.That(userCommunity!.Status, Is.EqualTo(UserCommunityStatus.Approved));
        Assert.That(userCommunity.UserId, Is.EqualTo(TestUser.Id));
        Assert.That(userCommunity.CommunityId, Is.EqualTo(community.Id));
    }
    
    [Test]
    public async Task ApproveUserAsync_WhenAlreadyJoined_ShouldFail()
    {
        var userCommunity = await _userCommunityService
            .ApproveUserAsync(AdminUser.Id, TestUser.Id, CommunityOne.Id);
        
        Assert.That(userCommunity, Is.EqualTo(null));
    }
    
    [Test]
    public async Task GetByUserAsync_WhenAlreadyJoined_ShouldWorkAsExpected()
    {
        var _ = await _userCommunityService
            .JoinCommunityAsync(TestUser.Id, CommunityTwo.Id);
        
        var __ = await _userCommunityService
            .JoinCommunityAsync(TestUser.Id, CommunityOne.Id);

        var communities = await _userCommunityService
            .GetAllByUser<UserCommunityModel>(TestUser.Id);
        
        Assert.That(communities.Count(), Is.EqualTo(2));
        Assert.That(communities.First().Id, Is.EqualTo(CommunityOne.Id));
        Assert.That(communities.Last().Id, Is.EqualTo(CommunityTwo.Id));
    }
}