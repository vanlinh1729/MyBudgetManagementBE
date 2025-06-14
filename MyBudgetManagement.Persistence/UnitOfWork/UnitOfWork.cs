using Microsoft.EntityFrameworkCore.Storage;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepositoryAsync Users { get; }
    public IUserBalanceRepositoryAsync UserBalances { get; }
    public ITransactionRepositoryAsync Transactions { get; }
    public ICategoryRepositoryAsync Categories { get; }
    public IDebtAndLoanRepositoryAsync DebtAndLoans { get; }
    public IDebtAndLoanContactRepositoryAsync DebtAndLoanContacts { get; }
    public IRoleRepositoryAsync Roles { get; }
    public IPermissionRepositoryAsync Permissions { get; }
    public ITokenRepositoryAsync Tokens { get; }
    public IGroupRepositoryAsync Groups { get; }
    public IGroupMemberRepositoryAsync GroupMembers { get; }
    public IGroupExpenseRepositoryAsync GroupExpenses { get; }
    public IGroupExpenseShareRepositoryAsync GroupExpenseShares { get; }
    public IGroupInvitationRepositoryAsync GroupInvitations { get; }
    public INotificationRepositoryAsync Notifications { get; }
    public IGroupMessageRepositoryAsync GroupMessages { get; }

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
        await _currentTransaction?.CommitAsync()!;
    }

    public async Task RollbackAsync()
    {
        await _currentTransaction?.RollbackAsync()!;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}