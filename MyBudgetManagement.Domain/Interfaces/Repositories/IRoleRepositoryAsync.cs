using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Roles;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface IRoleRepositoryAsync : IGenericRepositoryAsync<Role>
{
    Task<Role?> GetRoleByRoleNameAsync(string roleName);

}