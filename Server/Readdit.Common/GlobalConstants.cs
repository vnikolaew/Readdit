using System.Reflection.Metadata;

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
    
    public static class Claims
    {
        public const string CountryClaim = "Country";
    }
}