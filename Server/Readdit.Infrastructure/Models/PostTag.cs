using System.ComponentModel.DataAnnotations;

namespace Readdit.Infrastructure.Models;

public class PostTag
{
    [Required]
    public string PostId { get; set; }
    
    public CommunityPost Post { get; set; }
        
    [Required]
    public string TagId { get; set; }

    public Tag Tag { get; set; }
}