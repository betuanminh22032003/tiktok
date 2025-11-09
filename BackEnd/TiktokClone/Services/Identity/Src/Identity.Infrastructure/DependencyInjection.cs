using FluentValidation;
using Identity.Application;
using Identity.Domain.Repositories;
using Identity.Infrastructure.Caching;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TiktokClone.SharedKernel.Application;
using TiktokClone.SharedKernel.Infrastructure;

namespace Identity.Infrastructure;

/// <summary>
/// Dependency injection configuration for Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("IdentityDb"),
                b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));

        // Redis
        var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
        services.AddScoped<ICacheService, RedisCacheService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork>(provider =>
        {
            var context = provider.GetRequiredService<IdentityDbContext>();
            var mediator = provider.GetRequiredService<IMediator>();
            return new UnitOfWork(context, mediator);
        });

        // Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // JWT Settings
        services.Configure<Security.JwtSettings>(options =>
        {
            var section = configuration.GetSection("JwtSettings");
            options.Key = section["SecretKey"] ?? section["Key"] ?? string.Empty;
            options.Issuer = section["Issuer"] ?? string.Empty;
            options.Audience = section["Audience"] ?? string.Empty;
            options.ExpiryMinutes = int.TryParse(section["ExpirationInMinutes"], out var exp) ? exp : 60;
        });

        return services;
    }
}
