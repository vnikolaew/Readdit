using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Readdit.Services.Data.Posts.Models;

public class CreatePostInputModel
{
    [Required]
    public string CommunityId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }

    public IEnumerable<string> Tags { get; set; } = new List<string>();
    
    public IFormFile? Media { get; set; }
}