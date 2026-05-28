using LitePress.Infrastructure.Marten;
using Marten;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace LitePress.Integration.Tests.Infrastructure;

/// <summary>
/// Shared xUnit fixture for API integration tests. Starts PostgreSQL via Testcontainers,
/// applies Marten schema, and hosts <see cref="Program"/> in memory with a shared <see cref="HttpClient"/>.
/// </summary>
public sealed class ApiIntegrationFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = PostgreSqlContainerFactory.Build();

    /// <summary>In-process Web API factory for resolving services and creating clients.</summary>
    public WebApplicationFactory<Program> Factory { get; private set; } = null!;

    /// <summary>Reusable HTTP client for tests in the <see cref="ApiIntegrationCollection"/>.</summary>
    public HttpClient Client { get; private set; } = null!;

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var connectionString = PostgreSqlContainerFactory.BuildConnectionString(_postgres);
        await ApplySchemaAsync(connectionString);

        SetTestEnvironment(connectionString);

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development"));

        Client = Factory.CreateClient();
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        Client?.Dispose();
        if (Factory is not null)
        {
            await Factory.DisposeAsync();
        }

        await _postgres.DisposeAsync();
    }

    /// <summary>Points the Web API at the Testcontainer and disables startup schema apply (fixture applies schema instead).</summary>
    internal static void SetTestEnvironment(string connectionString)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", connectionString);
        Environment.SetEnvironmentVariable("Database__ApplySchemaOnStartup", "false");
        Environment.SetEnvironmentVariable("JwtSettings__Secret", TestAuth.DevJwtSecret);
    }

    private static async Task ApplySchemaAsync(string connectionString)
    {
        await using var store = DocumentStore.For(options =>
        {
            options.Connection(connectionString);
            MartenStoreRegistration.ConfigureStore(options);
        });

        await store.Storage.ApplyAllConfiguredChangesToDatabaseAsync();
    }
}
