using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities.Roles;

namespace MyBudgetManagement.Domain.Entities.Users;

public class UserRole : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    //nav props
    
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}