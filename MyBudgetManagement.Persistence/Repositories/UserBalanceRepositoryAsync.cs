using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class UserBalanceRepositoryAsync : GenericRepositoryAsync<UserBalance>, IUserBalanceRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public UserBalanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.UserBalances)
    {
        _dbContext = dbContext;
    }

    public async Task<UserBalance> GetUserBalanceByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public Task<UserBalance> GetUserBalanceWithTransactionsAsync(Guid userBalanceId)
    {
        throw new NotImplementedException();
    }

    public Task<UserBalance> GetUserBalanceWithCategoriesAsync(Guid userBalanceId)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetCurrentBalanceAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}