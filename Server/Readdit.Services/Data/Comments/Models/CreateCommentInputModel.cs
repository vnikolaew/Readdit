using System.ComponentModel.DataAnnotations;
using static Readdit.Common.GlobalConstants.Comment;

namespace Readdit.Services.Data.Comments.Models;

public class CreateCommentInputModel
{
    [Required]
    public string PostId { get; set; }
    
    [Required]
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; }
}