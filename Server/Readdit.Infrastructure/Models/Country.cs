using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Models;

public class Country : BaseEntity<string>
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}