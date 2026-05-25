using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LitePress.Infrastructure.Persistence;

public static class DatabaseMigrationExtensions
{
    /// <summary>
    /// Applies pending EF Core migrations in Development only when
    /// Database:ApplyMigrationsOnStartup is true (default in Development).
    /// Production deployments must use reviewed migration artifacts.
    /// </summary>
    public static async Task ApplyDevelopmentMigrationsAsync(
        this IServiceProvider services,
        IHostEnvironment environment,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        if (!environment.IsDevelopment())
        {
            return;
        }

        if (string.Equals(
                configuration["Database:ApplyMigrationsOnStartup"],
                "false",
                StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LitePressDbContext>();
        await db.Database.MigrateAsync(cancellationToken);
    }
}
