using System.ComponentModel.DataAnnotations;
using static Readdit.Common.GlobalConstants.Community;

namespace Readdit.Services.Data.Communities.Models;

public class CreateCommunityInputModel : BaseCommunityInputModel
{
    [Required]
    [MinLength(NameMinLength)]
    [MaxLength(NameMaxLength)]
    [RegularExpression(NameRegex)]
    public string Name { get; set; }
}