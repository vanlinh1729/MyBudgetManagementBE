using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities.Groups;

public class GroupMember : BaseEntity
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
    public Guid? InvitationId { get; set; } 
    public bool IsGroupLeader { get; set; } // Nếu true -> Group Leader
    public GroupMemberStatus Status { get; set; } 
    public DateTime JoinDate { get; set; }
    public decimal NetBalance { get; set; }

    // Navigation properties
    public virtual Group Group { get; set; }
    public virtual User User { get; set; }
    public virtual GroupInvitation? Invitation { get; set; }
    public virtual ICollection<GroupExpense> Expenses { get; set; } = new List<GroupExpense>();
    public virtual ICollection<GroupExpenseShare> ExpenseShares { get; set; } = new List<GroupExpenseShare>();

}