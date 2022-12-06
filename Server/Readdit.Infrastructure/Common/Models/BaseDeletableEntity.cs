namespace Readdit.Infrastructure.Common.Models;

public abstract class BaseDeletableEntity<TKey> : BaseAuditableEntity<TKey>, IDeletableEntity
{
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedOn { get; set; }
}