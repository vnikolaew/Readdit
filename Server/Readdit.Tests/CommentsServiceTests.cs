using Moq;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Comments;
using Readdit.Services.Data.Scores;
using Readdit.Tests.Common;

namespace Readdit.Tests;

[TestFixture]
public class CommentsServiceTests
{
    private static CommentsService _commentsService;
    private static Mock<IPostScoresService> _postScoresMock;
    private static Mock<ICommentScoreService> _commentScoresMock;
    
    private static readonly ApplicationUser TestUser = new()
    {
        UserName = "JDoe",
        Id = "TestId",
        FirstName = "John",
        LastName = "Doe"
    }; 
    
    private static readonly ApplicationUser AdminUser = new()
    {
        UserName = "AdminUser",
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

    private static readonly CommunityPost Post = new()
    {
        Author = TestUser,
        Community = CommunityOne,
        Title = "Test Title",
        Content = "Test Content",
        MediaUrl = "Media Url",
        MediaPublicId = "Media Public Id",
    };

    private static IRepository<UserCommunity> _userCommunityRepo;

    [OneTimeSetUp]
    public void SetUp()
    {
        var context = InMemoryDbContextProvider.Instance;
        
        var postsRepo = DbRepositoryProvider.GetDeletable<CommunityPost>();
        var usersRepo = DbRepositoryProvider.GetDeletable<ApplicationUser>();
        _userCommunityRepo = DbRepositoryProvider.GetDeletable<UserCommunity>();
        var postCommentsRepo = DbRepositoryProvider.GetDeletable<PostComment>();
        var commentVotesRepo = DbRepositoryProvider.GetDeletable<CommentVote>();

        _postScoresMock = new Mock<IPostScoresService>();
        _postScoresMock
            .Setup(ps => ps.IncreaseForUserAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _postScoresMock
            .Setup(ps => ps.DecreaseForUserAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        
        _commentScoresMock = new Mock<ICommentScoreService>();
        _commentScoresMock
            .Setup(cs => cs.IncreaseForUserAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _commentScoresMock
            .Setup(cs => cs.DecreaseForUserAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        
        _commentsService = new CommentsService(
            postCommentsRepo, usersRepo, postsRepo, _userCommunityRepo, _postScoresMock.Object, _commentScoresMock.Object, commentVotesRepo);
        
        usersRepo.Add(TestUser);
        usersRepo.Add(AdminUser);
        
        context.Communities.Add(CommunityOne);
        context.SaveChanges();
        
        postsRepo.Add(Post);
        context.SaveChanges();
    }

    [Test]
    public async Task Adding_NewComment_WhenAuthor_IsCommunityMember_ShouldWorkAsExpected()
    {
        _userCommunityRepo.Add(new UserCommunity
        {
            User = TestUser,
            Community = CommunityOne,
            Status = UserCommunityStatus.Approved
        });
        await _userCommunityRepo.SaveChangesAsync();

        var comment = await _commentsService.CreateAsync(TestUser.Id, Post.Id, "Test Content");
        Assert.That(comment, Is.Not.Null);
        
        Assert.That(comment!.Author.UserName, Is.EqualTo(TestUser.UserName));
        Assert.That(comment!.Post.Title, Is.EqualTo(Post.Title));
        Assert.That(comment.PostId, Is.EqualTo(Post.Id));
        Assert.That(comment.AuthorId, Is.EqualTo(TestUser.Id));
        Assert.That(comment.VoteScore, Is.EqualTo(0));
        
    }
    
    [Test]
    public async Task Adding_NewComment_WhenAuthor_IsNotApproved_ShouldFail()
    {
        _userCommunityRepo.Add(new UserCommunity
        {
            User = TestUser,
            Community = CommunityOne,
            Status = UserCommunityStatus.Pending
        });
        await _userCommunityRepo.SaveChangesAsync();

        var comment = await _commentsService.CreateAsync(TestUser.Id, Post.Id, "Test Content");
        Assert.That(comment, Is.Null);
        _postScoresMock.Verify(ps => ps.IncreaseForUserAsync(
            It.IsAny<string>(), It.IsAny<int>()), Times.Never());
    }
    
    [Test]
    public async Task Updating_Comment_WhenAuthor_IsAuthor_ShouldWorkAsExpected()
    {
        _userCommunityRepo.Add(new UserCommunity
        {
            User = TestUser,
            Community = CommunityOne,
            Status = UserCommunityStatus.Approved
        });
        await _userCommunityRepo.SaveChangesAsync();

        var comment = await _commentsService.CreateAsync(TestUser.Id, Post.Id, "Test Content");

        comment = await _commentsService.UpdateAsync(TestUser.Id, comment!.Id, "Updated comment");
        
        Assert.That(comment, Is.Not.Null);
        
        Assert.That(comment.Content, Is.EqualTo("Updated comment"));
        Assert.That(comment.AuthorId, Is.EqualTo(TestUser.Id));
        Assert.That(comment.PostId, Is.EqualTo(Post.Id));
    }
    
    [Test]
    public async Task Updating_Comment_WhenAuthor_IsNotAnAuthor_ShouldFail()
    {
        _userCommunityRepo.Add(new UserCommunity
        {
            User = AdminUser,
            Community = CommunityOne,
            Status = UserCommunityStatus.Approved
        });
        await _userCommunityRepo.SaveChangesAsync();
        
        var comment = await _commentsService
            .CreateAsync(AdminUser.Id, Post.Id, "Test Content");

        Console.WriteLine(comment);
        
        comment = await _commentsService
            .UpdateAsync(TestUser.Id, comment!.Id, "Updated comment");
        
        Assert.That(comment, Is.Null);
    }
    
    [Test]
    public async Task Deleting_Comment_WhenAuthor_IsAuthor_ShouldWorkAsExpected()
    {
        _userCommunityRepo.Add(new UserCommunity
        {
            User = TestUser,
            Community = CommunityOne,
            Status = UserCommunityStatus.Approved
        });
        await _userCommunityRepo.SaveChangesAsync();

        var comment = await _commentsService.CreateAsync(TestUser.Id, Post.Id, "Test Content");

        var success = await _commentsService.DeleteAsync(TestUser.Id, comment!.Id);
        
        Assert.That(success, Is.True);
        
        _commentScoresMock.Verify(cs => cs.DecreaseForUserAsync(
            comment.AuthorId, comment.VoteScore), Times.Once());
        _postScoresMock.Verify(ps => ps.DecreaseForUserAsync(
            comment.Post.AuthorId, 1), Times.Once());
    }
}