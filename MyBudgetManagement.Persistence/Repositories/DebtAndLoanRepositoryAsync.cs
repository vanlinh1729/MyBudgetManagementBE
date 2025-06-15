using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Debts;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class DebtAndLoanRepositoryAsync : GenericRepositoryAsync<DebtAndLoan>, IDebtAndLoanRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public DebtAndLoanRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.DebtAndLoans)
    {
        _dbContext = dbContext;
    }
}