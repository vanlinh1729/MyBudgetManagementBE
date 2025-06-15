using System.Collections;
using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities.Transactions;

namespace MyBudgetManagement.Domain.Entities.Users;

public class UserBalance : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    
    //nav props
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; }
}