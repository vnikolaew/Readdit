using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Countries.Models;

public class CountryModel : IMapFrom<Country>
{
    public string Code { get; set; }
    
    public string Name { get; set; }
}