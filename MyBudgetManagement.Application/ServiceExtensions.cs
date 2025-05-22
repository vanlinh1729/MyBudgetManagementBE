using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

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