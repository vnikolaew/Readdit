using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Readdit.Services.Data.Posts.Models;

public class UpdatePostInputModel
{
    [Required]
    public string PostId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }
    
    public IFormFile Media { get; set; }
}