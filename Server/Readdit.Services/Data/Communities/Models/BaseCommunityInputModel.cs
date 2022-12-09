using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Services.Data.Communities.Models;

public abstract class BaseCommunityInputModel
{
    [Required]
    public string Description { get; set; }
    
    public IFormFile? Picture { get; set; }

    public IEnumerable<string> Tags { get; set; } = new List<string>();
    
    [Required]
    public CommunityType Type { get; set; }
}