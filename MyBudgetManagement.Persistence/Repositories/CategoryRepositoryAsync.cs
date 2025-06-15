using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Categories;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class CategoryRepositoryAsync : GenericRepositoryAsync<Category>, ICategoryRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext, dbContext.Categories)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId)
    {
        return await _dbContext.Categories
            .Include(c => c.User) // Include related data if needed
            .Where(x => x.User.Id == userId)
            .AsNoTracking() // Optional: for better performance if you're only reading
            .ToListAsync();
    }
    public Task<IEnumerable<Category>> GetCategoriesByUserBalanceIdAsync(Guid userBalanceId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Category>> GetCategoriesByTypeAsync(Guid userId, CategoryType type)
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetCategoryWithTransactionsAsync(Guid categoryId)
    {
        throw new NotImplementedException();
    }
}