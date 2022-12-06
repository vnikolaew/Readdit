namespace Readdit.Infrastructure.Common.Models;

public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>, IAuditableEntity
{
    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
}