using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities.Users;

namespace MyBudgetManagement.Domain.Entities.Roles;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    //nav props
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}