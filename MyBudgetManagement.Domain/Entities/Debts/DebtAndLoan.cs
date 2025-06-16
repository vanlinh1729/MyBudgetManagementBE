using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities.Categories;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities.Debts;

public class DebtAndLoan : AuditableBaseEntity
{
    public Guid DebtContactId { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsDebt { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public string? Image { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string Note { get; set; }   
    //nav props
    public virtual Category Category { get; set; }
    
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual DebtAndLoanContact DebtAndLoanContact { get; set; }
    
}