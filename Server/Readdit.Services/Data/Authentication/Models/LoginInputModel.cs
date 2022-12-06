using System.ComponentModel.DataAnnotations;

namespace Readdit.Services.Data.Authentication.Models;

public class LoginInputModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}