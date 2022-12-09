using System.ComponentModel.DataAnnotations;

namespace Readdit.Services.Data.Comments.Models;

public class CreateCommentInputModel
{
    [Required]
    public string PostId { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; }
}