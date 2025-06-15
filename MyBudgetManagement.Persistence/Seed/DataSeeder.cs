using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Roles;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Persistence.Seed;

public class DataSeeder : IDataSeeder
{
    private readonly IUnitOfWork _uow;

    public DataSeeder(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task SeedAsync()
    {
        await _uow.BeginTransactionAsync();
        try
        {
            await SeedRolesAsync();
            await SeedPermissionsAsync();
            await SeedRolePermissionsAsync();
            await SeedUsersAsync();
            await SeedUserRolesAsync();
            await SeedUserBalancesAsync();

            
            await _uow.CommitAsync();
            Console.WriteLine("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync();
            Console.WriteLine($"Database seeding failed: {ex.Message}");
            throw; // Re-throw to ensure the error is not silently ignored
        }
    }

    private async Task SeedRolesAsync()
    {
        if (await _uow.Roles.Query().AnyAsync()) return;

        var roles = new List<Role>
        {
            new() { Name = "SuperAdmin", Description = "Full system access" },
            new() { Name = "Admin", Description = "Administrative access" },
            new() { Name = "Manager", Description = "Team management access" },
            new() { Name = "User", Description = "Standard user access" },
            new() { Name = "Guest", Description = "Limited access" }
        };

        await _uow.Roles.BulkInsertAsync(roles);
        await _uow.SaveChangesAsync();
    }

    private async Task SeedPermissionsAsync()
    {
        if (await _uow.Permissions.Query().AnyAsync()) return;

        var permissions = new List<Permission>
        {
            new() { Name = "Users.Create", Description = "Create users" },
            new() { Name = "Users.Read", Description = "View users" },
            new() { Name = "Users.Update", Description = "Edit users" },
            new() { Name = "Users.Delete", Description = "Delete users" },
            
            new() { Name = "Roles.Create", Description = "Create roles" },
            new() { Name = "Roles.Read", Description = "View roles" },
            new() { Name = "Roles.Update", Description = "Edit roles" },
            new() { Name = "Roles.Delete", Description = "Delete roles" },
            
            new() { Name = "Transactions.Create", Description = "Create transactions" },
            new() { Name = "Transactions.Read", Description = "View transactions" },
            new() { Name = "Transactions.Update", Description = "Edit transactions" },
            new() { Name = "Transactions.Delete", Description = "Delete transactions" }
        };

        await _uow.Permissions.BulkInsertAsync(permissions);
        await _uow.SaveChangesAsync();
    }

    
    private async Task SeedRolePermissionsAsync()
    {
        if (await _uow.RolePermissions.Query().AnyAsync()) return;

        var roles = await _uow.Roles.Query().ToListAsync();
        var permissions = await _uow.Permissions.Query().ToListAsync();

        var rolePermissions = new List<RolePermission>();

        // SuperAdmin gets all permissions
        var superAdmin = roles.First(r => r.Name == "SuperAdmin");
        foreach (var permission in permissions)
        {
            rolePermissions.Add(new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = superAdmin.Id,
                PermissionId = permission.Id
            });
        }

        // Admin gets most permissions except sensitive ones
        var admin = roles.First(r => r.Name == "Admin");
        var adminPermissions = permissions.Where(p => !p.Name.Contains("Roles.Delete"));
        foreach (var permission in adminPermissions)
        {
            rolePermissions.Add(new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = admin.Id,
                PermissionId = permission.Id
            });
        }

        // Manager gets transaction-related permissions
        var manager = roles.First(r => r.Name == "Manager");
        var managerPermissions = permissions.Where(p => p.Name.Contains("Transactions"));
        foreach (var permission in managerPermissions)
        {
            rolePermissions.Add(new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = manager.Id,
                PermissionId = permission.Id
            });
        }

        // User gets basic permissions
        var user = roles.First(r => r.Name == "User");
        var userPermissions = permissions.Where(p => 
            p.Name == "Transactions.Create" || 
            p.Name == "Transactions.Read");
        foreach (var permission in userPermissions)
        {
            rolePermissions.Add(new RolePermission
            {              
                Id = Guid.NewGuid(),
                RoleId = user.Id,
                PermissionId = permission.Id
            });
        }

        await _uow.RolePermissions.BulkInsertAsync(rolePermissions);
        await _uow.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        var existingUsers = await _uow.Users.Query()
            .Where(u => new[] { "superadmin@example.com", "admin@example.com", "user@example.com" }
                .Contains(u.Email))
            .ToListAsync();

        if (existingUsers.Count >= 3) return;

        var users = new List<User>
        {
            new() 
            { 
                Email = "superadmin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin123!"),
                FullName = "Super Admin",
                Status = AccountStatus.Activated
            },
            new() 
            { 
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                FullName = "Administrator",
                Status = AccountStatus.Activated
            },
            new() 
            { 
                Email = "user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                FullName = "Regular User",
                Status = AccountStatus.Activated
            }
        };

        await _uow.Users.BulkInsertAsync(users);
        await _uow.SaveChangesAsync();

    }

    private async Task SeedUserRolesAsync()
    {
        var checkUserRoleExists = await _uow.UserRoles.Query().AnyAsync();
        if (checkUserRoleExists) return;

        // Get required users
    var superAdminUser = await _uow.Users.Query()
        .FirstOrDefaultAsync(u => u.Email == "superadmin@example.com");
    var adminUser = await _uow.Users.Query()
        .FirstOrDefaultAsync(u => u.Email == "admin@example.com");
    var regularUser = await _uow.Users.Query()
        .FirstOrDefaultAsync(u => u.Email == "user@example.com");

    // Get required roles
    var superAdminRole = await _uow.Roles.Query()
        .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
    var adminRole = await _uow.Roles.Query()
        .FirstOrDefaultAsync(r => r.Name == "Admin");
    var userRole = await _uow.Roles.Query()
        .FirstOrDefaultAsync(r => r.Name == "User");

    // Verify all required data exists
    var missingData = new List<string>();
    if (superAdminUser == null) missingData.Add("superadmin@example.com user");
    if (adminUser == null) missingData.Add("admin@example.com user");
    if (regularUser == null) missingData.Add("user@example.com user");
    if (superAdminRole == null) missingData.Add("SuperAdmin role");
    if (adminRole == null) missingData.Add("Admin role");
    if (userRole == null) missingData.Add("User role");

    if (missingData.Any())
    {
        throw new InvalidOperationException(
            $"Missing required data for seeding user roles: {string.Join(", ", missingData)}. " +
            "Please ensure users and roles are seeded first.");
    }

    var userRoles = new List<UserRole>
    {
        new() 
        { 
            Id = Guid.NewGuid(),
            UserId = superAdminUser.Id,
            RoleId = superAdminRole.Id,
        },
        new() 
        { 
            Id = Guid.NewGuid(),
            UserId = adminUser.Id,
            RoleId = adminRole.Id,
        },
        new() 
        { 
            Id = Guid.NewGuid(),
            UserId = regularUser.Id,
            RoleId = userRole.Id,
        }
    };

    await _uow.UserRoles.BulkInsertAsync(userRoles);
    await _uow.SaveChangesAsync();
    }
    

    private async Task SeedUserBalancesAsync()
    {
        var users = await _uow.Users.Query()
            .Where(u => new[] { "superadmin@example.com", "admin@example.com", "user@example.com" }
                .Contains(u.Email))
            .ToListAsync();

        if (await _uow.UserBalances.Query().AnyAsync(ub => users.Select(u => u.Id).Contains(ub.UserId)))
            return;

        var balances = users.Select(user => new UserBalance
        {
            UserId = user.Id,
            Balance = 0// Default starting balance
        }).ToList();

        await _uow.UserBalances.BulkInsertAsync(balances);
        await _uow.SaveChangesAsync();
    }
}