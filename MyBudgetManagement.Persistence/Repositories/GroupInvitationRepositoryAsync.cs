using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Groups;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class GroupInvitationRepositoryAsync : GenericRepositoryAsync<GroupInvitation>, IGroupInvitationRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupInvitationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.GroupInvitations)
    {
        _dbContext = dbContext;
    }
}