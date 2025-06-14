using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class PermissionRepositoryAsync : GenericRepositoryAsync<Permission>, IPermissionRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public PermissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.Permissions)
    {
        _dbContext = dbContext;
    }
    
}