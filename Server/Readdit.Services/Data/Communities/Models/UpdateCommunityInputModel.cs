using System.ComponentModel.DataAnnotations;

namespace Readdit.Services.Data.Communities.Models;

public class UpdateCommunityInputModel : BaseCommunityInputModel
{
    [Required]
    public string CommunityId { get; set; }
}