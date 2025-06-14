using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class DebtAndLoanContactRepositoryAsync : GenericRepositoryAsync<DebtAndLoanContact>, IDebtAndLoanContactRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public DebtAndLoanContactRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.DebtAndLoanContacts)
    {
        _dbContext = dbContext;
    }
}