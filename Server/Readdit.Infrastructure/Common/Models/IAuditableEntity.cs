namespace Readdit.Infrastructure.Common.Models;

public interface IAuditableEntity
{
    DateTime CreatedOn { get; set; }
    
    DateTime? ModifiedOn { get; set; }
}