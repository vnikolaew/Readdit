using Microsoft.AspNetCore.Http;
using Moq;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Tags;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.External.Cloudinary.Models;

namespace Readdit.Tests.PostsService;

[TestFixture]
public class PostsServiceTests
{
    private static Services.Data.Posts.PostsService _postsService;
    private static Mock<ICloudinaryService> _cloudinaryServiceMock;

    private static readonly ApplicationUser TestUser = new()
    {
        Id = "TestId",
        FirstName = "John",
        LastName = "Doe"
    }; 
    
    private static readonly ApplicationUser TestUserTwo = new()
    {
        Id = "TestId2",
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

    [OneTimeSetUp]
    public async Task SetUp()
    {
        var postsRepo = DbRepositoryProvider.GetDeletable<CommunityPost>();
        var usersRepo = DbRepositoryProvider.GetDeletable<ApplicationUser>();
        var communityRepo = DbRepositoryProvider.GetDeletable<Community>();
        var userCommunityRepo = DbRepositoryProvider.GetDeletable<UserCommunity>();
        var postVotesRepo = DbRepositoryProvider.GetDeletable<PostVote>();
        var postCommentsRepo = DbRepositoryProvider.GetDeletable<PostComment>();
        var commentVotesRepo = DbRepositoryProvider.GetDeletable<CommentVote>();
        var postTagsRepo = DbRepositoryProvider.Get<PostTag>();

        _cloudinaryServiceMock = new Mock<ICloudinaryService>();
        _cloudinaryServiceMock.Setup(cs =>
                cs.UploadAsync(
                    It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ImageUploadResult { AbsoluteImageUrl = "Image Url", ImagePublidId = "Image Id" });
        var tagsService = new TagsService(DbRepositoryProvider.Get<Tag>());

        usersRepo.Add(TestUser);
        usersRepo.Add(TestUserTwo);
        usersRepo.Add(AdminUser);
        
        communityRepo.Add(CommunityOne);
        await usersRepo.SaveChangesAsync();
        
        userCommunityRepo.Add(new UserCommunity
        {
            User = TestUser,
            Community = CommunityOne,
            Status = UserCommunityStatus.Approved
        });
        await userCommunityRepo.SaveChangesAsync();
        
        _postsService = new Services.Data.Posts.PostsService(
            postsRepo,
            usersRepo,
            communityRepo,
            userCommunityRepo,
            _cloudinaryServiceMock.Object,
            postVotesRepo,
            postCommentsRepo,
            postTagsRepo,
            tagsService,
            null!,
            commentVotesRepo,
            null!
        );
    }

    [Test]
    public async Task CreatePost_ShouldWord_AsExpected_WhenUser_IsPartOfCommunity()
    {
        var post = await _postsService.CreateAsync(
            TestUser.Id,
            CommunityOne.Id,
            "Test Title",
            "Test Content",
            new string[] { },
            new FormFile(new MemoryStream(), 0, 1, "", "")
            {
                Headers = new HeaderDictionary(),
                ContentType = "",
            });
        
        Assert.That(post, Is.Not.EqualTo(null));
        Assert.That(post.AuthorId, Is.EqualTo(TestUser.Id));
        Assert.That(post.CommunityId, Is.EqualTo(CommunityOne.Id));
        Assert.That(post.Content, Is.EqualTo("Test Content"));
        Assert.That(post.Title, Is.EqualTo("Test Title"));
        Assert.That(post.MediaUrl, Is.EqualTo("Image Url"));
        Assert.That(post.MediaPublicId, Is.EqualTo("Image Id"));
    }

    [Test]
    public async Task CreatePost_ShouldNotWork_AsExpected_WhenUser_IsNotPartOfCommunity()
    {
        
        var post = await _postsService.CreateAsync(
            TestUserTwo.Id,
            CommunityOne.Id,
            "Test Title",
            "Test Content",
            new string[] { },
            new FormFile(new MemoryStream(), 0, 1, "", "")
            {
                Headers = new HeaderDictionary(),
                ContentType = "",
            });
        
        Assert.That(post, Is.EqualTo(null));
    }

    [Test]
    public async Task CreatePost_ShouldWork_WhenUser_IsPartOfCommunity_WithoutImage()
    {
        var post = await _postsService.CreateAsync(
            TestUser.Id,
            CommunityOne.Id,
            "Test Title",
            "Test Content",
            new string[] { },
            null!);
        
        Assert.That(post, Is.Not.EqualTo(null));
        Assert.That(post.AuthorId, Is.EqualTo(TestUser.Id));
        Assert.That(post.CommunityId, Is.EqualTo(CommunityOne.Id));
        Assert.That(post.Content, Is.EqualTo("Test Content"));
        Assert.That(post.Title, Is.EqualTo("Test Title"));
        Assert.That(post.MediaUrl, Is.EqualTo(string.Empty));
        Assert.That(post.MediaPublicId, Is.EqualTo(string.Empty));
        _cloudinaryServiceMock.Verify(cs =>
                cs.UploadAsync(
                    It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
    }
}