using LitePress.Integration.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LitePress.Integration.Tests;

public sealed class ScalarEndpointTests : IClassFixture<ApiIntegrationFixture>
{
    private readonly ApiIntegrationFixture _fixture;

    public ScalarEndpointTests(ApiIntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ScalarV1_ReturnsSuccess()
    {
        var response = await _fixture.Client.GetAsync("/scalar/v1");

        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task LegacyV1ScalarPath_RedirectsToScalarV1()
    {
        using var client = _fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        var response = await client.GetAsync("/v1/scalar");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Be("/scalar/v1");
    }
}
