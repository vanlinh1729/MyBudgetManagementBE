using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class NotificationRepositoryAsync : GenericRepositoryAsync<Notification>, INotificationRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.Notifications)
    {
        _dbContext = dbContext;
    }
}