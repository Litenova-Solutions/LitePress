using Microsoft.AspNetCore.Mvc.Testing;

namespace LiteNova.LitePress.Integration.Tests;

public sealed class OpenApiExportTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OpenApiExportTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("JwtSettings:Secret", "dev-secret-key-must-be-at-least-32-characters-long!");
            builder.UseSetting("ConnectionStrings:Database", "Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress");
        }).CreateClient();
    }

    [Fact]
    public async Task ExportOpenApi_WritesPackagesApiTypesFile()
    {
        var response = await _client.GetAsync("/openapi/v1.json");
        response.IsSuccessStatusCode.Should().BeTrue();

        var json = await response.Content.ReadAsStringAsync();
        var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../.."));
        var outputPath = Path.Combine(repoRoot, "packages", "api-types", "openapi.json");
        await File.WriteAllTextAsync(outputPath, json);
    }
}
