namespace Readdit.Common;

public static class GlobalConstants
{
    public const string AdministratorRoleName = "Admin";
    
    public const string RegularUserRoleName = "RegularUser";

    public const string ReadditEmail = "vikinik01@abv.bg";
    
    public const string ApplicationName = "Readdit";
    
    public static class Seeding
    {
        public const string CountryDataPath = @"../Readdit.Infrastructure/Data/Seeding/Data/Countries.json";
    }

    public static class Community
    {
        public const string NameRegex = @"^[A-Za-z][A-Za-z0-9_]*$";
        
        public const int NameMaxLength = 50;
        
        public const int NameMinLength = 3;
        
        public const int PicturePublicIdMaxLength = 50;
        
        public const int DescriptionMaxLength = 500;
    }
    
    public static class Post
    {
        public const int TitleMaxLength = 200;
        
        public const int TitleMinLength = 6;
        
        public const int ContentMaxLength = 200;
        
        public const int ContentMinLength = 6;
    }
    
    public static class User
    {
        public const int FirstNameMaxLength = 50;
        
        public const int FirstNameMinLength = 3;
        
        public const int LastNameMaxLength = 50;
        
        public const int LastNameMinLength = 3;
    }
    
    public static class Country
    {
        public const int NameMaxLength = 50;
        
        public const int CodeMaxLength = 6;
    }
    
    public static class Comment
    {
        public const int ContentMaxLength = 1000;
        
        public const int ContentMinLength = 3;
    }
    
    public static class UserProfile
    {
        public const int AboutContentMaxLength = 300;
    }
    
    public static class Defaults
    {
    }
    
    public static class Claims
    {
        public const string CountryClaim = "Country";
    }
}