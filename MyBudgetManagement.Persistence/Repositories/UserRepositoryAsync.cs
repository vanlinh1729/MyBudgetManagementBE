using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Common;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Roles;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;

public class UserRepositoryAsync : GenericRepositoryAsync<User>,IUserRepositoryAsync
{

    private readonly DbSet<Role> _roles;
    private readonly DbSet<UserRole> _userRoles;

    public UserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext,dbContext.Users)
    {
        _roles = dbContext.Set<Role>();
        _userRoles = dbContext.Set<UserRole>();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
       return await ExistsAsync(x=>x.Email == email);
    }

    public async Task AssignRoleAsync(Guid userId, string roleName, Guid actorId, CancellationToken cancellationToken = default)
    {
        var role = await _roles
            .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);

        if (role == null)
            throw new NotFoundException($"Vai trò '{roleName}' không tồn tại.");

        var alreadyExists = await _userRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == role.Id, cancellationToken);

        if (alreadyExists)
            return;

        var userRole = new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoleId = role.Id,
            Created = DateTime.UtcNow,
            CreatedBy = actorId
        };

        await _userRoles.AddAsync(userRole, cancellationToken);
    }
    // public async Task AddAsync(User entity)
    // {
    //     throw new NotImplementedException();
    // }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
       return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}