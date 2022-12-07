using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Models;

public class Country : BaseEntity<string>
{
    public Country()
    {
        Id = Guid.NewGuid().ToString();
    }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string Code { get; set; }
}