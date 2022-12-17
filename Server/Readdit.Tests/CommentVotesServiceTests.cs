using Moq;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.CommentVotes;
using Readdit.Services.Data.Common.UnitOfWork;
using Readdit.Services.Data.Scores;
using Readdit.Tests.Common;

namespace Readdit.Tests;

[TestFixture]
public class CommentVotesServiceTests
{
    private static CommentVotesService _commentVotesService;
    private static IRepository<CommentVote> _commentVotes;
    private static IRepository<PostComment> _comments;
    private static Mock<ICommentScoreService> _commentScoresMock;
    private static Mock<IUnitOfWork> _unitOfWorkMock;
    
    private static readonly ApplicationUser TestUserOne = new()
    {
        UserName = "JDoe",
        Id = "TestId",
        FirstName = "John",
        LastName = "Doe"
    }; 
    
    private static readonly ApplicationUser TestUserTwo = new()
    {
        UserName = "JDoe2",
        Id = "TestId2",
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
        Type = CommunityType.Public,
        Id = "CommunityId",
        Name = "TestCommunity", 
        Description = "Some description",
        PictureUrl = "Some Picture Url",
        PicturePublicId = "Some Picture Id"
    };

    private static readonly CommunityPost Post = new()
    {
        Community = CommunityOne,
        Title = "Test Title",
        Content = "Test Content",
        MediaUrl = "Media Url",
        MediaPublicId = "Media Public Id",
    };
    
    private static readonly PostComment Comment = new()
    {
        Post = Post,
        Content = "Test Content",
    };

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var context = InMemoryDbContextProvider.Instance;
        
        _commentVotes = DbRepositoryProvider.Get<CommentVote>();
        _comments = DbRepositoryProvider.Get<PostComment>();
        
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => context.SaveChangesAsync())
            .ReturnsAsync(1);
        
        _commentScoresMock = new Mock<ICommentScoreService>();
        _commentScoresMock.Setup(cs =>
                cs.IncreaseForUserAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);
        _commentScoresMock.Setup(cs =>
                cs.DecreaseForUserAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        _commentVotesService = new CommentVotesService(
            _commentVotes, _comments, _unitOfWorkMock.Object, _commentScoresMock.Object);
    }

    [SetUp]
    public void SetUp()
    {
        var context = InMemoryDbContextProvider.Instance;
        context.Database.EnsureDeleted();
        
        context.Users.AddRange(TestUserOne, TestUserTwo, AdminUser);
        context.SaveChanges();

        CommunityOne.Admin = AdminUser;
        context.Communities.Add(CommunityOne);
        context.SaveChanges();

        Post.Author = TestUserOne;
        context.CommunityPosts.Add(Post);
        context.SaveChanges();

        Comment.Author = TestUserTwo;
        context.PostComments.Add(Comment);
        context.SaveChanges();
    }

    [Test]
    public async Task UpVotingComment_ShouldWorkAsExpected()
    {
        var commentVote = await _commentVotesService.UpVoteAsync(TestUserOne.Id, Comment.Id);
        
        Assert.That(commentVote, Is.Not.Null);
        
        Assert.That(commentVote.Type, Is.EqualTo(VoteType.Up));
        Assert.That(commentVote.UserId, Is.EqualTo(TestUserOne.Id));
        Assert.That(commentVote.CommentId, Is.EqualTo(Comment.Id));
        Assert.That(Comment.VoteScore, Is.EqualTo(1));
        
        _commentScoresMock.Verify(cs =>
            cs.IncreaseForUserAsync(TestUserTwo.Id, 1), Times.Once());
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Test]
    public async Task DownVotingComment_ShouldWorkAsExpected()
    {
        var commentVote = await _commentVotesService.DownVoteAsync(TestUserOne.Id, Comment.Id);
        
        Assert.That(commentVote, Is.Not.Null);
        
        Assert.That(commentVote.Type, Is.EqualTo(VoteType.Down));
        Assert.That(commentVote.UserId, Is.EqualTo(TestUserOne.Id));
        Assert.That(commentVote.CommentId, Is.EqualTo(Comment.Id));
        Assert.That(Comment.VoteScore, Is.EqualTo(-1));
        
        _commentScoresMock.Verify(cs =>
            cs.DecreaseForUserAsync(TestUserTwo.Id, 1), Times.Once());
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Test]
    public async Task RemovingUpVote_ShouldWorkAsExpected()
    {
        var commentVote = await _commentVotesService
            .UpVoteAsync(TestUserOne.Id, Comment.Id);

        var success = await _commentVotesService.RemoveUpVoteAsync(TestUserOne.Id, Comment.Id);
        
        Assert.That(success, Is.True);
        Assert.That(Comment.VoteScore, Is.EqualTo(0));
        _commentScoresMock.Verify(cs =>
            cs.DecreaseForUserAsync(TestUserTwo.Id, 1), Times.Once());
    }
    
    [Test]
    public async Task RemovingDownVote_ShouldWorkAsExpected()
    {
        var commentVote = await _commentVotesService
            .DownVoteAsync(TestUserOne.Id, Comment.Id);

        var success = await _commentVotesService.RemoveDownVoteAsync(TestUserOne.Id, Comment.Id);
        
        Assert.That(success, Is.True);
        Assert.That(Comment.VoteScore, Is.EqualTo(0));
        
        _commentScoresMock.Verify(cs =>
            cs.IncreaseForUserAsync(TestUserTwo.Id, 1), Times.Once());
    }
}