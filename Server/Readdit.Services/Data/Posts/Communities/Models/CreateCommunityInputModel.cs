using System.ComponentModel.DataAnnotations;
using Readdit.Services.Data.Posts.Communities.Models;

namespace Readdit.Services.Data.Communities.Models;

public class CreateCommunityInputModel : BaseCommunityInputModel
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; }
}