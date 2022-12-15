using System.ComponentModel.DataAnnotations;

namespace Readdit.Services.Data.Posts.Models;

public class CreatePostInputModel : BasePostInputModel
{
    [Required]
    public string CommunityId { get; set; }
}