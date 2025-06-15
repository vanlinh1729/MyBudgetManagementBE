using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities.Roles;

public class Permission : BaseEntity
{ 
    public string Name { get; set; }
    public string Description { get; set; }

    //nav props
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}