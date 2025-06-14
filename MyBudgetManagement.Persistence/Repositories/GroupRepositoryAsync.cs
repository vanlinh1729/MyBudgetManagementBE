using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class GroupRepositoryAsync : GenericRepositoryAsync<Group>, IGroupRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public GroupRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.Groups)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Group>> GetAllGroupByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }
}