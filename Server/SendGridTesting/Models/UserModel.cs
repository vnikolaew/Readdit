using Bogus.DataSets;

namespace SendGridTesting.Models;

public class UserModel
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public Name.Gender Gender { get; set; }
    
    public string About { get; set; }
    
    public string ProfilePictureUrl { get; set; }
}

public enum Gender : sbyte
{
    Male = 1,
    Female = 2,
    Other = 3
}