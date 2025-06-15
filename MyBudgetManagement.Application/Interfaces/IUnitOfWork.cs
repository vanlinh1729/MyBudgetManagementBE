using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;

namespace MyBudgetManagement.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepositoryAsync Users { get; }
    IUserBalanceRepositoryAsync UserBalances { get; }
    ITransactionRepositoryAsync Transactions { get; }
    ICategoryRepositoryAsync Categories { get; }
    IDebtAndLoanRepositoryAsync DebtAndLoans { get; }
    IDebtAndLoanContactRepositoryAsync DebtAndLoanContacts { get; }
    IRoleRepositoryAsync Roles { get; }
    IPermissionRepositoryAsync Permissions { get; }
    IRolePermissionRepositoryAsync RolePermissions { get; }
    IUserRoleRepositoryAsync UserRoles { get; }
    ITokenRepositoryAsync Tokens { get; }
    IGroupRepositoryAsync Groups { get; }
    IGroupMemberRepositoryAsync GroupMembers { get; }
    IGroupExpenseRepositoryAsync GroupExpenses { get; }
    IGroupExpenseShareRepositoryAsync GroupExpenseShares { get; }
    IGroupInvitationRepositoryAsync GroupInvitations { get; }
    INotificationRepositoryAsync Notifications { get; }
    IGroupMessageRepositoryAsync GroupMessages { get; }
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}