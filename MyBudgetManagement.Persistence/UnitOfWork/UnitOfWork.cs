using Microsoft.EntityFrameworkCore.Storage;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
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
    private IJwtTokenService _jwtTokenService;
    public IUserRepositoryAsync Users { get; }
    public IUserBalanceRepositoryAsync UserBalances { get; }
    public ITransactionRepositoryAsync Transactions { get; }
    public ICategoryRepositoryAsync Categories { get; }
    public IDebtAndLoanRepositoryAsync DebtAndLoans { get; }
    public IDebtAndLoanContactRepositoryAsync DebtAndLoanContacts { get; }
    public IRoleRepositoryAsync Roles { get; }
    public IRolePermissionRepositoryAsync RolePermissions { get; }
    
    public IUserRoleRepositoryAsync UserRoles { get; }
    public IPermissionRepositoryAsync Permissions { get; }
    public ITokenRepositoryAsync Tokens { get; }
    public IGroupRepositoryAsync Groups { get; }
    public IGroupMemberRepositoryAsync GroupMembers { get; }
    public IGroupExpenseRepositoryAsync GroupExpenses { get; }
    public IGroupExpenseShareRepositoryAsync GroupExpenseShares { get; }
    public IGroupInvitationRepositoryAsync GroupInvitations { get; }
    public INotificationRepositoryAsync Notifications { get; }
    public IGroupMessageRepositoryAsync GroupMessages { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new UserRepositoryAsync(_context);
        UserBalances = new UserBalanceRepositoryAsync(_context);
        Transactions = new TransactionRepositoryAsync(_context);
        Categories = new CategoryRepositoryAsync(_context);
        DebtAndLoans = new DebtAndLoanRepositoryAsync(_context);
        DebtAndLoanContacts = new DebtAndLoanContactRepositoryAsync(_context);
        Roles = new RoleRepositoryAsync(_context);
        Permissions = new PermissionRepositoryAsync(_context);
        RolePermissions = new RolePermissionRepositoryAsync(_context);
        UserRoles = new UserRoleRepositoryAsync(_context);
        Tokens = new TokenRepositoryAsync(_context,_jwtTokenService);
        Groups = new GroupRepositoryAsync(_context);
        GroupMembers = new GroupMemberRepositoryAsync(_context);
        GroupExpenses = new GroupExpenseRepositoryAsync(_context);
        GroupExpenseShares = new GroupExpenseShareRepositoryAsync(_context);
        GroupInvitations = new GroupInvitationRepositoryAsync(_context);
        Notifications = new NotificationRepositoryAsync(_context);
        GroupMessages = new GroupMessageRepositoryAsync(_context);
    }


   
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