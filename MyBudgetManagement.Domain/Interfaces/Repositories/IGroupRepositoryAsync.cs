using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Groups;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface IGroupRepositoryAsync : IGenericRepositoryAsync<Group>
{
    Task<IEnumerable<Group>> GetAllGroupByUserId(Guid userId);
}