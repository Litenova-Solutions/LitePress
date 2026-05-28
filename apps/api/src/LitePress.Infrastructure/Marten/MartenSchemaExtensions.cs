using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Host startup helpers for Marten storage schema (tables and indexes in PostgreSQL).
/// </summary>
public static class MartenSchemaExtensions
{
    /// <summary>
    /// Applies Marten storage schema when allowed by environment and <c>Database:ApplySchemaOnStartup</c>.
    /// Integration and acceptance tests set the flag to <c>false</c> and apply schema in their fixture instead.
    /// Invoked from <c>Program.cs</c> on startup and via <c>--apply-schema-only</c>.
    /// </summary>
    public static async Task ApplyDevelopmentSchemaAsync(
        this IServiceProvider services,
        IHostEnvironment environment,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        if (environment.IsProduction())
        {
            return;
        }

        var flag = configuration["Database:ApplySchemaOnStartup"];
        if (string.Equals(flag, "false", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (!environment.IsDevelopment()
            && !string.Equals(flag, "true", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        using var scope = services.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();
        await store.Storage.ApplyAllConfiguredChangesToDatabaseAsync();
    }
}
