using System.ComponentModel.DataAnnotations;

namespace Readdit.Services.Data.Communities.Models;

public class CreateCommunityInputModel : BaseCommunityInputModel
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; }
}