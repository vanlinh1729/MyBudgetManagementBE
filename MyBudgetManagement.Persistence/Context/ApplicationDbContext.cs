using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // USER -> CATEGORY
        modelBuilder.Entity<Category>()
            .HasOne(c => c.User)
            .WithMany(u => u.Categories) // thêm ICollection<Category> Categories trong User entity
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade); // xóa user thì xóa categories

// USER -> TRANSACTION
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions) // thêm ICollection<Transaction> Transactions trong User entity
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade); // xóa user thì xóa transactions

// CATEGORY -> TRANSACTION
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // không cho xóa category nếu có transaction

// USER -> USERBALANCE
        modelBuilder.Entity<UserBalance>()
            .HasOne(ub => ub.User)
            .WithOne(u => u.UserBalance)
            .HasForeignKey<UserBalance>(ub => ub.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }

    public DbSet<Category> Categories { get; set; }
    // public DbSet<DebtAndLoan> DebtAndLoans { get; set; }
    // public DbSet<DebtAndLoanContact> DebtAndLoanContacts { get; set; }
    // public DbSet<Group> Groups { get; set; }
    // public DbSet<GroupExpense> GroupExpenses { get; set; }
    // public DbSet<GroupExpenseShare> GroupExpenseShares { get; set; }
    // public DbSet<GroupInvitation> GroupInvitations { get; set; }
    // public DbSet<GroupMember> GroupMembers { get; set; }
    // public DbSet<GroupMessage> GroupMessages { get; set; }
    // public DbSet<Notification> Notifications { get; set; }
    // public DbSet<Permission> Permissions { get; set; }
    // public DbSet<Role> Roles { get; set; }
    // public DbSet<Token> Tokens { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserBalance> UserBalances { get; set; }
    
    public DbSet<Token> Tokens { get; set; }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await base.SaveChangesAsync(cancellationToken);
}