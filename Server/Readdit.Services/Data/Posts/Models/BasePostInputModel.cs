using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static Readdit.Common.GlobalConstants.Post;

namespace Readdit.Services.Data.Posts.Models;

public abstract class BasePostInputModel
{
    [Required]
    [MinLength(TitleMinLength)]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; }
        
    [Required]
    [MinLength(ContentMinLength)]
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; }

    public IEnumerable<string> Tags { get; set; } = new List<string>();
    
    public IFormFile? Media { get; set; }
}