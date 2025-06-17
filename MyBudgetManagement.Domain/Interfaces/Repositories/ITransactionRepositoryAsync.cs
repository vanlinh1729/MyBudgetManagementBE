using MyBudgetManagement.Domain.Entities.Transactions;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface ITransactionRepositoryAsync : IGenericRepositoryAsync<Transaction>
{
    void RemoveRange(IEnumerable<Transaction> transactions);

    Task UpdateTransactionsCategoryToNullAsync(Guid categoryId);
    Task DeleteTransactionAsync(Guid transactionId);
    Task<IReadOnlyList<Transaction>> GetTransactionsByCategoryId(Guid catId);
    Task<IReadOnlyList<Transaction>> GetTransactionsByUserId(Guid userId);

    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalTransactionsByCategoryAsync(Guid categoryId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetTransactionsByUserBalanceIdAsync(Guid userBalanceId);
}