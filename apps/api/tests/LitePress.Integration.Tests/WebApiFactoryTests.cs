using LitePress.Integration.Tests.Infrastructure;

namespace LitePress.Integration.Tests;

[Collection(ApiIntegrationCollection.Name)]
public sealed class WebApiFactoryTests
{
    private readonly HttpClient _client;

    public WebApiFactoryTests(ApiIntegrationFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GetOpenApi_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/openapi/v1.json");

        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
