using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using MyBudgetManagement.Application;
using MyBudgetManagement.Infrastructure;
using MyBudgetManagement.Persistence;
using MyBudgetManagement.Persistence.Seed;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;
using FluentValidation.AspNetCore;
using FluentValidation;
using MyBudgetManagement.Application.Features.Auth.Commands.Login;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Sink(new FileSink("Logs/logs.txt", new JsonFormatter(), long.MaxValue, Encoding.Default))
    .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

// Register services in the container
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
    
    // Add FluentValidation
    builder.Services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();
    
    // Register validators from Application assembly
    builder.Services.AddValidatorsFromAssemblyContaining<LoginCommand>();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MyBudgetManagement API",
            Version = "v1",
            Description = "API for managing budgets"
        });

        // Swagger Authorization configuration
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200") // Cho phép origin này
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
    builder.Host.UseSerilog();
    var app = builder.Build();

    //seed data
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        try
        {
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }
    
// Configure the HTTP request pipeline.
    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAllOrigins"); // Áp dụng CORS policy
    
    // Add Global Exception Handling
    app.UseMiddleware<MyBudgetManagement.API.Middlewares.GlobalExceptionMiddleware>();
    
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.MapControllers();
    
    // Seed data asynchronously
    await app.RunAsync();
    return 1;
}
catch (Exception ex)
{
    if (ex is HostAbortedException) throw;

    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 0;
}
finally
{
    Log.CloseAndFlush();
}
