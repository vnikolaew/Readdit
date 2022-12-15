using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static Readdit.Common.GlobalConstants.UserProfile;
using static Readdit.Common.GlobalConstants.User;

namespace Readdit.Services.Data.Users.Models;

public class UpdateUserProfileModel
{
    [MinLength(FirstNameMinLength)]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; }
    
    [MinLength(LastNameMinLength)]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; }
    
    public string Gender { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    [MaxLength(AboutContentMaxLength)]
    public string AboutContent { get; set; }
    
    public IFormFile? Picture { get; set; }
}