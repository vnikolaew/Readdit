using System.ComponentModel.DataAnnotations;

namespace Readdit.Infrastructure.Models;

public class CommunityTag
{
    [Required]
    public string CommunityId { get; set; }

    public Community Community { get; set; }

    [Required]
    public string TagId { get; set; }

    public Tag Tag { get; set; }
}