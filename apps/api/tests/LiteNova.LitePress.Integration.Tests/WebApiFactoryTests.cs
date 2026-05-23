using Microsoft.AspNetCore.Mvc.Testing;

namespace LiteNova.LitePress.Integration.Tests;

public sealed class WebApiFactoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WebApiFactoryTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("JwtSettings:Secret", "dev-secret-key-must-be-at-least-32-characters-long!");
            builder.UseSetting("ConnectionStrings:Database", "Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress");
        }).CreateClient();
    }

    [Fact]
    public async Task GetOpenApi_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/openapi/v1.json");

        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
