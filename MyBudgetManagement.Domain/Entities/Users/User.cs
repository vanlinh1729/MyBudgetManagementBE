using System.Net;
using System.Security.AccessControl;
using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities.Categories;
using MyBudgetManagement.Domain.Entities.Groups;
using MyBudgetManagement.Domain.Entities.Notifications;
using MyBudgetManagement.Domain.Entities.Roles;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities.Users;

public class User : AuditableBaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; }
    public string? Avatar { get; set; }
    public Gender Gender { get; set; } 
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? LastChangePassword { get; set; }
    public AccountStatus Status { get; set; }
    public Currencies Currency { get; set; }

    // Navigation properties
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
    // public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual UserBalance UserBalance { get; set; }
    public virtual ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
    public virtual ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<GroupInvitation> SentInvitations { get; set; } = new List<GroupInvitation>();
    public virtual ICollection<GroupInvitation> ReceivedInvitations { get; set; } = new List<GroupInvitation>();
    
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
}