using Microsoft.Identity.Client;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class GroupExpenseRepositoryAsync : GenericRepositoryAsync<GroupExpense>, IGroupExpenseRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupExpenseRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.GroupExpenses)
    {
        _dbContext = dbContext;
    }
}