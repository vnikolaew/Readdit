using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;
using static Readdit.Common.GlobalConstants.Country;

namespace Readdit.Infrastructure.Models;

public class Country : BaseEntity<string>
{
    public Country()
    {
        Id = Guid.NewGuid().ToString();
    }
    
    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(CodeMaxLength)]
    public string Code { get; set; }
}