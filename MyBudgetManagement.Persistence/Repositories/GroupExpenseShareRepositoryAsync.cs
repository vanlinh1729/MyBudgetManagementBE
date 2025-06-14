using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class GroupExpenseShareRepositoryAsync : GenericRepositoryAsync<GroupExpenseShare>, IGroupExpenseShareRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupExpenseShareRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.GroupExpenseShares)
    {
        _dbContext = dbContext;
    }
}