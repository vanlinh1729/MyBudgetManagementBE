using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities.Categories;
using MyBudgetManagement.Domain.Entities.Debts;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities.Transactions;

public class Transaction : AuditableBaseEntity
{
    public Guid CategoryId { get; set; }
    public Guid UserBalanceId { get; set; }
    
    public Guid? DebtAndLoanId { get; set; }
    public decimal Amount { get; set; }
    public string? Image { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }    

    
    //nav props
    public virtual Category Category { get; set; }
    public virtual UserBalance UserBalance { get; set; }
    public virtual DebtAndLoan? DebtAndLoan { get; set; }
}