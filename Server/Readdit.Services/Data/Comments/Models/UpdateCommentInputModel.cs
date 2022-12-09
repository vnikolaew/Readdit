using System.ComponentModel.DataAnnotations;

namespace Readdit.Services.Data.Comments.Models;

public class UpdateCommentInputModel
{
    [Required]
    public string CommentId { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; }
}