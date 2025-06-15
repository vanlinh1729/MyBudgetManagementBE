using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Domain.Entities.Roles;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class RolePermissionRepositoryAsync : GenericRepositoryAsync<RolePermission>, IRolePermissionRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public RolePermissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.RolePermissions)
    {
        _dbContext = dbContext;
    }
}