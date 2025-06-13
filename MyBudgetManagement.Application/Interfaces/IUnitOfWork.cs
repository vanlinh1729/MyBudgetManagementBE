using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepositoryAsync Users { get; }
    IUserBalanceRepositoryAsync UserBalances { get; }
    ITransactionRepositoryAsync Transactions { get; }
    ICategoryRepositoryAsync Categories { get; }
    IDebtAndLoanRepositoryAsync DebtAndLoans { get; }
    IDebtAndLoanContactRepositoryAsync DebtAndLoanContacts { get; }
    IRoleRepositoryAsync Roles { get; }
    IPermissionRepositoryAsync Permissions { get; }
    ITokenRepositoryAsync Tokens { get; }
    IGroupRepositoryAsync Groups { get; }
    IGroupMemberRepositoryAsync GroupMembers { get; }
    IGroupExpenseRepositoryAsync GroupExpenses { get; }
    IGroupExpenseShareRepositoryAsync GroupExpenseShares { get; }
    IGroupInvitationRepositoryAsync GroupInvitations { get; }
    INotificationRepositoryAsync Notifications { get; }
    IGroupMessageRepositoryAsync GroupMessages { get; }
    
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    
    
}