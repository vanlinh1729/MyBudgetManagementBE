using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyBudgetManagement.Application.Common.Interfaces;

namespace MyBudgetManagement.Application;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        /*
        services.AddScoped<IUserService, UserService>();
    */

    }}