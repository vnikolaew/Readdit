using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Models;

public class Tag : BaseAuditableEntity<string>
{
    public Tag()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string Name { get; set; }
}