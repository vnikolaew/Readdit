using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Countries;

public interface ICountryService
{
    Task<IEnumerable<T>> GetAllAsync<T>();
    
    Task<Country?> GetByNameAsync(string name);
    
    Task<Country?> GetByUserAsync(string userId);
}