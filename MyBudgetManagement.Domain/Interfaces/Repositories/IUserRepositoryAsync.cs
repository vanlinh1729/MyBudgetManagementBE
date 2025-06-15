using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Users;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface IUserRepositoryAsync : IGenericRepositoryAsync<User>
{
    Task<bool> EmailExistsAsync(string email);
    Task AssignRoleAsync(Guid userId, string roleName, Guid actorId, CancellationToken cancellationToken = default);

    Task<User?> GetUserByEmailAsync(string email);
}