using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TiktokClone.SharedKernel.Application;
using TiktokClone.SharedKernel.Infrastructure;
using Video.Domain.Repositories;
using Video.Infrastructure.Caching;
using Video.Infrastructure.Persistence;
using Video.Infrastructure.Persistence.Repositories;

namespace Video.Infrastructure;

/// <summary>
/// Dependency injection configuration for Video Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddVideoInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<VideoDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("VideoDb"),
                b => b.MigrationsAssembly(typeof(VideoDbContext).Assembly.FullName)));

        // Redis (if not already registered)
        var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        if (!services.Any(x => x.ServiceType == typeof(IConnectionMultiplexer)))
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
        }

        // Cache Service (if not already registered)
        if (!services.Any(x => x.ServiceType == typeof(ICacheService)))
        {
            services.AddScoped<ICacheService, RedisCacheService>();
        }

        // Repositories
        services.AddScoped<IVideoRepository, VideoRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork>(provider =>
        {
            var context = provider.GetRequiredService<VideoDbContext>();
            var mediator = provider.GetRequiredService<IMediator>();
            return new UnitOfWork(context, mediator);
        });

        return services;
    }
}
