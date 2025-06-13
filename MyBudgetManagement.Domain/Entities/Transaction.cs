using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities;

public class Transaction : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Image { get; set; }
    public DateTime Date { get; set; }
    public string Note { get; set; }    

    
    //nav props
    public virtual Category Category { get; set; }
    public virtual User User { get; set; }
}