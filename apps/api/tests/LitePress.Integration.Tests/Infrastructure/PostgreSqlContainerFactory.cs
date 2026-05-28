using Testcontainers.PostgreSql;

namespace LitePress.Integration.Tests.Infrastructure;

/// <summary>
/// Builds a PostgreSQL <see cref="PostgreSqlContainer"/> for integration tests.
/// Shared by <see cref="ApiIntegrationFixture"/> so all tests in the collection use one database instance.
/// </summary>
internal static class PostgreSqlContainerFactory
{
    /// <summary>Creates a container definition (not started until <see cref="PostgreSqlContainer.StartAsync"/>).</summary>
    public static PostgreSqlContainer Build() =>
        new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("litepress")
            .WithUsername("litepress")
            .WithPassword("litepress")
            .Build();

    /// <summary>Returns the Npgsql connection string for a running container.</summary>
    public static string BuildConnectionString(PostgreSqlContainer container) =>
        container.GetConnectionString();
}
