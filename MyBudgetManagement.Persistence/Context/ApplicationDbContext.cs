using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Categories;
using MyBudgetManagement.Domain.Entities.Debts;
using MyBudgetManagement.Domain.Entities.Groups;
using MyBudgetManagement.Domain.Entities.Notifications;
using MyBudgetManagement.Domain.Entities.Roles;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Entities.Users;

namespace MyBudgetManagement.Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // User configurations
    modelBuilder.Entity<User>(entity =>
    {
        entity.HasMany(u => u.Tokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(u => u.Categories)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(u => u.UserBalance)
            .WithOne(ub => ub.User)
            .HasForeignKey<UserBalance>(ub => ub.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(u => u.GroupMemberships)
            .WithOne(gm => gm.User)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(u => u.ReceivedNotifications)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.NoAction); // Changed to NoAction to avoid multiple cascade paths

        entity.HasMany(u => u.SentNotifications)
            .WithOne(n => n.Sender)
            .HasForeignKey(n => n.SenderId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasMany(u => u.SentInvitations)
            .WithOne(gi => gi.Inviter)
            .HasForeignKey(gi => gi.InviterId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasMany(u => u.ReceivedInvitations)
            .WithOne(gi => gi.Invitee)
            .HasForeignKey(gi => gi.InviteeId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // UserBalance configuration
    modelBuilder.Entity<UserBalance>(entity =>
    {
        entity.HasOne(ub => ub.User)
            .WithOne(u => u.UserBalance)
            .HasForeignKey<UserBalance>(ub => ub.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // Token configuration
    modelBuilder.Entity<Token>(entity =>
    {
        entity.HasOne(t => t.User)
            .WithMany(u => u.Tokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // Transaction configuration
    modelBuilder.Entity<Transaction>(entity =>
    {
        entity.HasOne(t => t.UserBalance)
            .WithMany(ub => ub.Transactions)
            .HasForeignKey(t => t.UserBalanceId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict to avoid multiple cascade paths
    });

    // Category configuration
    modelBuilder.Entity<Category>(entity =>
    {
        entity.HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(c => c.DebtAndLoans)
            .WithOne(d => d.Category)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    // Role configuration
    modelBuilder.Entity<Role>(entity =>
    {
        entity.Property(r => r.Name).IsRequired().HasMaxLength(50);

        // Corrected Role-Permission many-to-many relationship
        entity.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // Group configurations
    modelBuilder.Entity<GroupMember>(entity =>
    {
        entity.HasOne(gm => gm.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict

        entity.HasOne(gm => gm.User)
            .WithMany(u => u.GroupMemberships)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(gm => gm.Invitation)
            .WithMany()
            .HasForeignKey(gm => gm.InvitationId)
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(gm => gm.Expenses)
            .WithOne(ge => ge.Member)
            .HasForeignKey(ge => ge.GroupMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(gm => gm.ExpenseShares)
            .WithOne(ges => ges.Member)
            .HasForeignKey(ges => ges.GroupMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    // Group configuration
    modelBuilder.Entity<Group>(entity =>
    {
        entity.HasMany(g => g.Members)
            .WithOne(gm => gm.Group)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Restrict); // Consistent with GroupMember configuration

        entity.HasMany(g => g.Expenses)
            .WithOne(ge => ge.Group)
            .HasForeignKey(ge => ge.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(g => g.Invitations)
            .WithOne(gi => gi.Group)
            .HasForeignKey(gi => gi.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(g => g.Messages)
            .WithOne(gm => gm.Group)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // GroupInvitation configuration
    modelBuilder.Entity<GroupInvitation>(entity =>
    {
        entity.HasOne(gi => gi.Group)
            .WithMany(g => g.Invitations)
            .HasForeignKey(gi => gi.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(gi => gi.Inviter)
            .WithMany(u => u.SentInvitations)
            .HasForeignKey(gi => gi.InviterId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(gi => gi.Invitee)
            .WithMany(u => u.ReceivedInvitations)
            .HasForeignKey(gi => gi.InviteeId)
            .OnDelete(DeleteBehavior.NoAction);
    });

    // GroupExpense configuration
    modelBuilder.Entity<GroupExpense>(entity =>
    {
        entity.HasOne(ge => ge.Group)
            .WithMany(g => g.Expenses)
            .HasForeignKey(ge => ge.GroupId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(ge => ge.Member)
            .WithMany(gm => gm.Expenses)
            .HasForeignKey(ge => ge.GroupMemberId)
            .OnDelete(DeleteBehavior.NoAction);
        
    });

    // GroupExpenseShare configuration
    modelBuilder.Entity<GroupExpenseShare>(entity =>
    {
        entity.HasOne(ges => ges.Group)
            .WithMany()
            .HasForeignKey(ges => ges.GroupId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(ges => ges.Member)
            .WithMany(gm => gm.ExpenseShares)
            .HasForeignKey(ges => ges.GroupMemberId)
            .OnDelete(DeleteBehavior.NoAction);
    });

    // GroupMessage configuration
    modelBuilder.Entity<GroupMessage>(entity =>
    {
        entity.HasOne(gm => gm.Group)
            .WithMany(g => g.Messages)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(gm => gm.GroupMember)
            .WithMany()
            .HasForeignKey(gm => gm.GroupMemberId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(gm => gm.ParentMessage)
            .WithMany()
            .HasForeignKey(gm => gm.ParentMessageId)
            .OnDelete(DeleteBehavior.NoAction);
    });

    // Notification configuration
    modelBuilder.Entity<Notification>(entity =>
    {
        entity.HasOne(n => n.User)
            .WithMany(u => u.ReceivedNotifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(n => n.Sender)
            .WithMany(u => u.SentNotifications)
            .HasForeignKey(n => n.SenderId)
            .OnDelete(DeleteBehavior.NoAction);
    });

    // DebtAndLoanContact configuration
    modelBuilder.Entity<DebtAndLoanContact>(entity =>
    {
        entity.HasMany(dlc => dlc.DebtAndLoans)
            .WithOne(dl => dl.DebtAndLoanContact)
            .HasForeignKey(dl => dl.DebtContactId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // DebtAndLoan configuration
    modelBuilder.Entity<DebtAndLoan>(entity =>
    {
        entity.HasOne(dl => dl.DebtAndLoanContact)
            .WithMany(dlc => dlc.DebtAndLoans)
            .HasForeignKey(dl => dl.DebtContactId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(dl => dl.Category)
            .WithMany(c => c.DebtAndLoans)
            .HasForeignKey(dl => dl.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    });
    // Permission configurations
    modelBuilder.Entity<Permission>(entity =>
    {
        entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
        entity.Property(p => p.Description).HasMaxLength(200);

        entity.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    });
    // UserRole configurations (junction table)
    modelBuilder.Entity<UserRole>(entity =>
    {
        entity.HasKey(ur => new { ur.UserId, ur.RoleId });

        entity.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        entity.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    });

    // RolePermission configurations (junction table)
    modelBuilder.Entity<RolePermission>(entity =>
    {
        entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        entity.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        entity.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);
    });
}
    public DbSet<Category> Categories { get; set; }
    public DbSet<DebtAndLoan> DebtAndLoans { get; set; }
    public DbSet<DebtAndLoanContact> DebtAndLoanContacts { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupExpense> GroupExpenses { get; set; }
    public DbSet<GroupExpenseShare> GroupExpenseShares { get; set; }
    public DbSet<GroupInvitation> GroupInvitations { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<RolePermission> RolePermissions { get; set; }
    
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserBalance> UserBalances { get; set; }
    
    public DbSet<Token> Tokens { get; set; }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await base.SaveChangesAsync(cancellationToken);
}