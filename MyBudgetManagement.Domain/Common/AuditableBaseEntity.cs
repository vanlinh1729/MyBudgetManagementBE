namespace MyBudgetManagement.Domain.Common;

public abstract class AuditableBaseEntity : BaseEntity
{
    
    public DateTime Created { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}