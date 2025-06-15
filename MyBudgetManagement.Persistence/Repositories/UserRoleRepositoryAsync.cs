using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class UserRoleRepositoryAsync : GenericRepositoryAsync<UserRole>, IUserRoleRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public UserRoleRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.UserRoles)
    {
        _dbContext = dbContext;
    }
}