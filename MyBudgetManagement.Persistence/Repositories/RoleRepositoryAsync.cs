using System.Data.Entity;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class RoleRepositoryAsync : GenericRepositoryAsync<Role>, IRoleRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public RoleRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.Roles)
    {
        _dbContext = dbContext;
    }

    public async Task<Role?> GetRoleByRoleNameAsync(string roleName)
    {
        return _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
    }
}