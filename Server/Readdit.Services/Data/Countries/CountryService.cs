using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Countries;

public class CountryService : ICountryService
{
    private readonly IRepository<Country> _countries;
    private readonly IRepository<ApplicationUser> _users;

    public CountryService(
        IRepository<Country> countries,
        IRepository<ApplicationUser> users)
    {
        _countries = countries;
        _users = users;
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>()
        => await _countries
            .AllAsNoTracking()
            .To<T>()
            .ToListAsync();

    public async Task<Country?> GetByNameAsync(string name)
        => await _countries.All().FirstOrDefaultAsync(c => c.Name == name);

    public async Task<Country?> GetByUserAsync(string userId)
        => (await _users
            .AllAsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.Country })
            .FirstOrDefaultAsync())?.Country;
}