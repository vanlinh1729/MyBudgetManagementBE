using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface IGroupRepositoryAsync : IGenericRepositoryAsync<Group>
{
    Task<IEnumerable<Group>> GetAllGroupByUserId(Guid userId);
}