namespace LitePress.Integration.Tests;

[Collection(Infrastructure.ApiIntegrationCollection.Name)]
public sealed class OpenApiExportTests
{
    private readonly HttpClient _client;

    public OpenApiExportTests(Infrastructure.ApiIntegrationFixture fixture)
    {
        _client = fixture.Client;
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
