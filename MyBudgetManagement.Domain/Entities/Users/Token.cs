using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Domain.Entities.Users;

public class Token : BaseEntity 
{
    public Guid UserId { get; set; }
    public string TokenValue { get; set; }
    public TokenType Type { get; set; }
    public DateTime ExpireAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    
    //nav props
    public virtual User User { get; set; }
}