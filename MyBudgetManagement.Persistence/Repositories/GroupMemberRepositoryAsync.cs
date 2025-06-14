using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class GroupMemberRepositoryAsync : GenericRepositoryAsync<GroupMember>, IGroupMemberRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupMemberRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.GroupMembers)
    {
        _dbContext = dbContext;
    }
}