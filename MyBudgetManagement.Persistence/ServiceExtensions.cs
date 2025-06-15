using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;
using MyBudgetManagement.Persistence.Repositories;
using MyBudgetManagement.Persistence.Seed;

namespace MyBudgetManagement.Persistence;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IDataSeeder, DataSeeder>();
        services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
        services.AddScoped<IUserRepositoryAsync, UserRepositoryAsync>();
        services.AddScoped<IUserBalanceRepositoryAsync, UserBalanceRepositoryAsync>();  
        services.AddScoped<ITransactionRepositoryAsync, TransactionRepositoryAsync>();  
        services.AddScoped<ICategoryRepositoryAsync, CategoryRepositoryAsync>();  
        services.AddScoped<IRoleRepositoryAsync, RoleRepositoryAsync>();  
        services.AddScoped<IPermissionRepositoryAsync, PermissionRepositoryAsync>();  
        services.AddScoped<ITokenRepositoryAsync, TokenRepositoryAsync>();  
        services.AddScoped<IGroupRepositoryAsync, GroupRepositoryAsync>();  
        services.AddScoped<IGroupMemberRepositoryAsync, GroupMemberRepositoryAsync>();  
        services.AddScoped<IGroupExpenseRepositoryAsync, GroupExpenseRepositoryAsync>();  

        
        return services;
    }
}