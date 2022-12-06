using System.ComponentModel.DataAnnotations;

namespace Readdit.Infrastructure.Common.Models;

public abstract class BaseEntity<TKey>
{
    [Key]
    public TKey Id { get; set; }
}