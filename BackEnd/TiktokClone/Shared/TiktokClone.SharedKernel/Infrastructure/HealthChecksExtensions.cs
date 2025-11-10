using Microsoft.Extensions.DependencyInjection;

namespace TiktokClone.SharedKernel.Infrastructure;

public static class HealthChecksExtensions
{
    /// <summary>
    /// Registers minimal health checks used by services. Services can call this helper
    /// to expose `/health` endpoints via ASP.NET Core Health Checks.
    /// </summary>
    public static IServiceCollection AddSharedHealthChecks(this IServiceCollection services)
    {
        // This helper is intentionally minimal to avoid forcing a health-check package
        // reference into the SharedKernel. Individual services should add concrete
        // health checks (e.g., database, redis) in their own projects.
        // Use this extension as a registration placeholder.
        return services;
    }
}
