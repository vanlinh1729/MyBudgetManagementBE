using System.Linq.Expressions;
using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IGenericRepositoryAsync<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(Guid id);

    
    Task BulkInsertAsync(IEnumerable<T> entities);
    
    Task<PagedResult<T>> GetPagedAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10);
}