using LitePress.Infrastructure.Marten;
using Marten;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace LitePress.AcceptanceTests.Support;

/// <summary>
/// Starts a disposable PostgreSQL Testcontainer and a <see cref="WebApplicationFactory{Program}"/>
/// for the full Reqnroll acceptance test run. Registered once in <see cref="Hooks.AcceptanceTestHooks.BeforeTestRun"/>.
/// </summary>
public sealed class AcceptanceTestWebAppFactory : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = PostgreSqlContainerFactory.Build();

    /// <summary>Npgsql connection string to the ephemeral test database.</summary>
    public string ConnectionString { get; private set; } = null!;

    /// <summary>In-process API host used to create <see cref="HttpClient"/> instances per scenario.</summary>
    public WebApplicationFactory<Program> Factory { get; private set; } = null!;

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        ConnectionString = PostgreSqlContainerFactory.BuildConnectionString(_postgres);
        await ApplySchemaAsync(ConnectionString);

        SetTestEnvironment(ConnectionString);

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development"));
    }

    /// <summary>Creates a new HTTP client for one BDD scenario (isolated cookie/header state).</summary>
    public HttpClient CreateScenarioClient() => Factory.CreateClient();

    /// <summary>Clears all Marten documents so the next scenario starts from an empty database.</summary>
    public Task ResetScenarioDataAsync(CancellationToken cancellationToken = default) =>
        ScenarioDatabase.ResetAsync(ConnectionString, cancellationToken);

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        if (Factory is not null)
        {
            await Factory.DisposeAsync();
        }

        await _postgres.DisposeAsync();
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

    private static void SetTestEnvironment(string connectionString)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", connectionString);
        Environment.SetEnvironmentVariable("Database__ApplySchemaOnStartup", "false");
        Environment.SetEnvironmentVariable("JwtSettings__Secret", TestUsers.DevJwtSecret);
    }
}
