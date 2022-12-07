using System.ComponentModel.DataAnnotations;
using Readdit.Services.Data.Posts.Communities.Models;

namespace Readdit.Services.Data.Communities.Models;

public class UpdateCommunityInputModel : BaseCommunityInputModel
{
    [Required]
    public string CommunityId { get; set; }
}