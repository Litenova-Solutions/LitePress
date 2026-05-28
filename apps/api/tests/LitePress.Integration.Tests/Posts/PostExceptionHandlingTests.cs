using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LitePress.Integration.Tests.Infrastructure;

namespace LitePress.Integration.Tests.Posts;

[Collection(ApiIntegrationCollection.Name)]
public sealed class PostExceptionHandlingTests
{
    private readonly HttpClient _client;

    public PostExceptionHandlingTests(ApiIntegrationFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task DeletePublishedPost_Returns409WithProblemDetails()
    {
        var postId = await CreateDraftPostAsync("Delete published conflict test");

        var publishResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, $"/api/posts/{postId}/publish").WithBearer());

        publishResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var deleteResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Delete, $"/api/posts/{postId}").WithBearer());

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        deleteResponse.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        using var json = await JsonDocument.ParseAsync(await deleteResponse.Content.ReadAsStreamAsync());
        var root = json.RootElement;

        root.GetProperty("status").GetInt32().Should().Be((int)HttpStatusCode.Conflict);
        root.GetProperty("detail").GetString().Should().Contain("cannot be deleted");
    }

    private async Task<string> CreateDraftPostAsync(string title)
    {
        var createResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, "/api/posts")
            {
                Content = JsonContent.Create(new
                {
                    title,
                    content = "{\"type\":\"doc\",\"content\":[{\"type\":\"paragraph\",\"content\":[{\"type\":\"text\",\"text\":\"Body\"}]}]}",
                    excerpt = "Integration excerpt",
                    coverImageUrl = (string?)null,
                    tagIds = Array.Empty<string>(),
                }),
            }.WithBearer());

        createResponse.IsSuccessStatusCode.Should().BeTrue();
        using var json = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        return json.RootElement.GetProperty("postId").GetString()!;
    }
}
