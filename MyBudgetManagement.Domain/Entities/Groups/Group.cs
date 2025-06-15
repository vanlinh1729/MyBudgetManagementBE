using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Entities.Groups;

public class Group : AuditableBaseEntity
{
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string Description { get; set; }
    public decimal TotalSpent { get; set; }
    
    // Navigation properties
    public virtual ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    public virtual ICollection<GroupExpense> Expenses { get; set; } = new List<GroupExpense>();
    public virtual ICollection<GroupInvitation> Invitations { get; set; } = new List<GroupInvitation>();
    public virtual ICollection<GroupMessage> Messages { get; set; } = new List<GroupMessage>();

}