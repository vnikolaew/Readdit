using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Scores;

namespace Readdit.Services.Data.Comments;

public class CommentsService : ICommentsService
{
    private readonly IDeletableEntityRepository<PostComment> _postComments;
    private readonly IRepository<ApplicationUser> _users;
    private readonly IRepository<CommunityPost> _posts;
    private readonly IRepository<UserCommunity> _userCommunities;
    private readonly IDeletableEntityRepository<CommentVote> _commentVotes;
    private readonly IPostScoresService _postScores;
    private readonly ICommentScoreService _commentScores;

    public CommentsService(
        IDeletableEntityRepository<PostComment> postComments,
        IRepository<ApplicationUser> users,
        IRepository<CommunityPost> posts,
        IRepository<UserCommunity> userCommunities,
        IPostScoresService postScores,
        ICommentScoreService commentScores,
        IDeletableEntityRepository<CommentVote> commentVotes)
    {
        _postComments = postComments;
        _users = users;
        _posts = posts;
        _userCommunities = userCommunities;
        _postScores = postScores;
        _commentScores = commentScores;
        _commentVotes = commentVotes;
    }

    public async Task<PostComment?> CreateAsync(
        string authorId, string postId, string content)
    {
        var author = await _users
            .All()
            .FirstOrDefaultAsync(u => u.Id == authorId);
        if (author is null)
        {
            return null;
        }

        var post = await _posts
            .All()
            .FirstOrDefaultAsync(p => p.Id == postId);
        if (post is null)
        {
            return null;
        }

        var authorIsCommunityMember = await _userCommunities
            .AllAsNoTracking()
            .AnyAsync(uc => uc.UserId == authorId
                            && uc.CommunityId == post.CommunityId
                            && uc.Status == UserCommunityStatus.Approved);
        if (!authorIsCommunityMember)
        {
            return null;
        }

        var comment = new PostComment
        {
            Author = author,
            Post = post,
            Content = content,
        };
        
        _postComments.Add(comment);
        await _postScores.IncreaseForUserAsync(post.AuthorId, 1);
        await _postComments.SaveChangesAsync();
        
        return comment;
    }

    public async Task<PostComment?> UpdateAsync(
        string authorId, string commentId, string content)
    {
        var existingComment = await _postComments
            .All()
            .FirstOrDefaultAsync(c => c.AuthorId == authorId
                                      && c.Id == commentId);
        if (existingComment is null)
        {
            return null;
        }

        existingComment.Content = content;
        _postComments.Update(existingComment);
        await _postComments.SaveChangesAsync();
        
        return existingComment;
    }

    public async Task<bool> DeleteAsync(string authorId, string commentId)
    {
        var existingComment = await _postComments
            .All()
            .Where(c => c.AuthorId == authorId
                                      && c.Id == commentId)
            .Select(c => new
            {
                Comment = c,
                PostAuthorId = c.Post.AuthorId,
                CommentVotes = c.Votes
            })
            .FirstOrDefaultAsync();
        
        if (existingComment is null)
        {
            return false;
        }

        foreach (var commentVote in existingComment.CommentVotes)
        {
            _commentVotes.Delete(commentVote);
        }
        
        _postComments.Delete(existingComment.Comment);
        await _postScores.DecreaseForUserAsync(existingComment.PostAuthorId, 1);
        await _commentScores.DecreaseForUserAsync(existingComment.Comment.AuthorId, existingComment.Comment.VoteScore);
        await _postComments.SaveChangesAsync();
        
        return true;
    }
}