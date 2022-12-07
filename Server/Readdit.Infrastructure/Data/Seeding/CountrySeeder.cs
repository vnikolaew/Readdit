using System.Text.Json;
using System.Text.Json.Serialization;
using Readdit.Infrastructure.Models;
using static Readdit.Common.GlobalConstants.Seeding;

namespace Readdit.Infrastructure.Data.Seeding;

public class CountrySeeder : ISeeder
{
    public async Task SeedAsync(ReadditDbContext context, IServiceProvider serviceProvider)
    {
        if (context.Countries.Any())
        {
            return;
        }

        var countries = GetCountries();
        foreach (var country in countries)
        {
            context.Countries.Add(country);
        }

        await context.SaveChangesAsync();
    }

    private static List<Country> GetCountries()
    {
        return JsonSerializer.Deserialize<List<CountryData>>(
                File.ReadAllText(CountryDataPath), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!
            .Select(c => new Country
            {
                Name = c.Name,
                Code = c.Code
            }).ToList();
    }
    
    private class CountryData
    {
        public string Name { get; set; }
        
        public string Code { get; set; }
    }
}