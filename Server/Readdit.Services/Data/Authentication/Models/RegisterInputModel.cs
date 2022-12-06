using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Readdit.Services.Data.Authentication.Models;

public class RegisterInputModel
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [MaxLength(30)]
    public string Password { get; set; }

    [Required]
    public string Country { get; set; }
    
    [Required]
    public string Gender { get; set; }
    
    public IFormFile? ProfilePicture { get; set; }
}