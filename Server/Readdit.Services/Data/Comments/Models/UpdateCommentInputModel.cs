using System.ComponentModel.DataAnnotations;
using static Readdit.Common.GlobalConstants.Comment;

namespace Readdit.Services.Data.Comments.Models;

public class UpdateCommentInputModel
{
    [Required]
    public string CommentId { get; set; }
    
    [Required]
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; }
}