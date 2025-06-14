using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class UserRepositoryAsync : GenericRepositoryAsync<User>,IUserRepositoryAsync
{

    public UserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.Users)
    {
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
       return await ExistsAsync(x=>x.Email == email);
    }

    public async Task AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
       return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}