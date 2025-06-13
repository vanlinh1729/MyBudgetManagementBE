using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Infrastructure.AuthService;
using MyBudgetManagement.Infrastructure.FileStorage;
using MyBudgetManagement.Infrastructure.JwtProvider;

namespace MyBudgetManagement.Infrastructure;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //add services
         services.AddScoped<IJwtTokenService, JwtTokenService>();  
         services.AddScoped<IPasswordHasher, PasswordHasher.PasswordHasher>();  
        /*
        services.AddTransient<IDataSeeder, DataSeeder>();
        */
        services.AddTransient<IEmailService, EmailService.EmailService>();
        services.AddTransient<IFileStorageService, CloudinaryService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        // Validate Cloudinary configuration
        var cloudinarySection = configuration.GetSection("Cloudinary");
        if (!cloudinarySection.Exists())
        {
            throw new Exception("Cloudinary configuration section is missing");
        }
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        services.AddAuthorization();


    }
}