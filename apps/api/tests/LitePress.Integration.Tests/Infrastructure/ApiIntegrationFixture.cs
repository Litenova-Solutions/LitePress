using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace LitePress.Integration.Tests.Infrastructure;

public sealed class ApiIntegrationFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    public WebApplicationFactory<Program> Factory { get; private set; } = null!;
    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
            builder.UseSetting("ConnectionStrings:Database", _postgres.GetConnectionString());
            builder.UseSetting("Database:ApplyMigrationsOnStartup", "true");
            builder.UseSetting("JwtSettings:Secret", TestAuth.DevJwtSecret);
        });

        Client = Factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}
