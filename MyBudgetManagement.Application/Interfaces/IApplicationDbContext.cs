
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Categories;
using MyBudgetManagement.Domain.Entities.Debts;
using MyBudgetManagement.Domain.Entities.Groups;
using MyBudgetManagement.Domain.Entities.Notifications;
using MyBudgetManagement.Domain.Entities.Roles;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Entities.Users;

namespace MyBudgetManagement.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<DebtAndLoan> DebtAndLoans { get; set; }
    DbSet<DebtAndLoanContact> DebtAndLoanContacts { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<GroupExpense> GroupExpenses { get; set; }
    DbSet<GroupExpenseShare> GroupExpenseShares { get; set; }
    DbSet<GroupInvitation> GroupInvitations { get; set; }
    DbSet<GroupMember> GroupMembers { get; set; }
    DbSet<GroupMessage> GroupMessages { get; set; }
    DbSet<Notification> Notifications { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<Role> Roles { get; set; }
     DbSet<Token> Tokens { get; set; }
     DbSet<RolePermission> RolePermissions { get; set; }
     DbSet<UserRole> UserRoles { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserBalance> UserBalances { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}