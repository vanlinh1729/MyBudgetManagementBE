using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface IUserBalanceRepositoryAsync : IGenericRepositoryAsync<UserBalance>
{
    Task<UserBalance> GetUserBalanceByUserIdAsync(Guid userId);
    Task<UserBalance> GetUserBalanceWithTransactionsAsync(Guid userBalanceId);
    Task<UserBalance> GetUserBalanceWithCategoriesAsync(Guid userBalanceId);
    Task<decimal> GetCurrentBalanceAsync(Guid userId);
}