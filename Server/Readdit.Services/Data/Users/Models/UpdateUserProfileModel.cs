using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Readdit.Services.Data.Users.Models;

public class UpdateUserProfileModel
{
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [MaxLength(50)]
    public string LastName { get; set; }
    
    public string Gender { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string AboutContent { get; set; }
    
    public IFormFile? Picture { get; set; }
}