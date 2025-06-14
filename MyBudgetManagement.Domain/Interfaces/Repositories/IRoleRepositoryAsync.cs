using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface IRoleRepositoryAsync : IGenericRepositoryAsync<Role>
{
    Task<Role?> GetRoleByRoleNameAsync(string roleName);

}