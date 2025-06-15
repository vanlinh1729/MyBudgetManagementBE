using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Groups;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class GroupMessageRepositoryAsync : GenericRepositoryAsync<GroupMessage>, IGroupMessageRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupMessageRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.GroupMessages)
    {
        _dbContext = dbContext;
    }
}