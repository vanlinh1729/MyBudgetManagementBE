using System.Linq.Expressions;
using MyBudgetManagement.Domain.Common;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IGenericRepositoryAsync<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

    Task AddAsync(T entity);

    void Update(T entity);

    void Remove(T entity);
    Task<bool> ExistsAsync(Expression<Func<T,bool>> predicate);

    
    Task BulkInsertAsync(IEnumerable<T> entities);
    
    Task<PagedResult<T>> GetPagedAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10);
    IQueryable<T> Query();

}