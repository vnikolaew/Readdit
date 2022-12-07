namespace Readdit.Common;

public static class GlobalConstants
{
    public const string AdministratorRoleName = "Admin";
    public const string RegularUserRoleName = "RegularUser";
    
    public static class Seeding
    {
        public static readonly string
            CountryDataPath = @"../Readdit.Infrastructure/Data/Seeding/Data/Countries.json";
    }
}