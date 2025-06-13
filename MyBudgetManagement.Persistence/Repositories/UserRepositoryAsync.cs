using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class UserRepositoryAsync : IUserRepositoryAsync
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepositoryAsync(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<User> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByUserBalanceAsync(Guid userBalanceId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserBalance.Id == userBalanceId);
    }

    public Task<bool> IsEmailUniqueAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserWithDetailsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> UserLogin(string email, string password)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Email == email);
        // Check if user exists and verify password
        if (user == null || !BCryptHelper.VerifyPassword(password, user.PasswordHash))
            return null;
        return user;
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task BulkInsertAsync(IEnumerable<User> entities)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<User>> GetPagedAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, int pageNumber = 1, int pageSize = 10)
    {
        throw new NotImplementedException();
    }
}