using System.Net.Http.Json;
using System.Text.Json;

namespace LitePress.AcceptanceTests.Support;

/// <summary>
/// Thin HTTP facade over the API for Reqnroll step definitions. One instance per scenario,
/// created in <see cref="Hooks.AcceptanceTestHooks.BeforeScenario"/>.
/// </summary>
public sealed class TestApiClient
{
    private readonly HttpClient _client;

    public TestApiClient(HttpClient client)
    {
        _client = client;
    }

    /// <summary>POST <c>/api/posts</c> as an authenticated author.</summary>
    public async Task<HttpResponseMessage> CreateDraftPostAsync(string title, string bearerToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/posts")
        {
            Content = JsonContent.Create(new
            {
                title,
                content = "{\"type\":\"doc\",\"content\":[{\"type\":\"paragraph\",\"content\":[{\"type\":\"text\",\"text\":\"Body\"}]}]}",
                excerpt = "Acceptance excerpt",
                coverImageUrl = (string?)null,
                tagIds = Array.Empty<string>(),
            }),
        };

        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

        return await _client.SendAsync(request);
    }

    /// <summary>Parses <c>postId</c> from a successful create response body.</summary>
    public async Task<string> ReadPostIdAsync(HttpResponseMessage createResponse)
    {
        createResponse.IsSuccessStatusCode.Should().BeTrue();
        using var json = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        return json.RootElement.GetProperty("postId").GetString()!;
    }

    /// <summary>POST <c>/api/posts/{id}/publish</c>. Optional bearer for unauthenticated publish scenarios.</summary>
    public async Task<HttpResponseMessage> PublishPostAsync(string postId, string? bearerToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/posts/{postId}/publish");

        if (bearerToken is not null)
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        }

        return await _client.SendAsync(request);
    }

    /// <summary>DELETE <c>/api/posts/{id}</c> as an authenticated author.</summary>
    public async Task<HttpResponseMessage> DeletePostAsync(string postId, string bearerToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/posts/{postId}");
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

        return await _client.SendAsync(request);
    }

    /// <summary>GET <c>/api/posts/{id}</c> (admin) with bearer token.</summary>
    public async Task<HttpResponseMessage> GetPostByIdAsync(string postId, string bearerToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/posts/{postId}");
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

        return await _client.SendAsync(request);
    }

    /// <summary>GET <c>/api/posts/{slug}</c> (public, no auth).</summary>
    public Task<HttpResponseMessage> GetPostBySlugAsync(string slug) =>
        _client.GetAsync($"/api/posts/{slug}");

    /// <summary>GET <c>/api/posts</c> published listing (public).</summary>
    public async Task<HttpResponseMessage> GetPublishedPostsAsync() =>
        await _client.GetAsync("/api/posts");

    /// <summary>POST <c>/api/tags</c> as an authenticated author.</summary>
    public async Task<HttpResponseMessage> CreateTagAsync(string name, string bearerToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/tags")
        {
            Content = JsonContent.Create(new { name }),
        };

        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

        return await _client.SendAsync(request);
    }

    /// <summary>Parses <c>tagId</c> from a successful create response body.</summary>
    public async Task<string> ReadTagIdAsync(HttpResponseMessage createResponse)
    {
        createResponse.IsSuccessStatusCode.Should().BeTrue();
        using var json = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        return json.RootElement.GetProperty("tagId").GetString()!;
    }

    /// <summary>GET <c>/api/tags</c> (authenticated list).</summary>
    public async Task<HttpResponseMessage> GetAllTagsAsync() =>
        await _client.GetAsync("/api/tags");

    /// <summary>Disposes the underlying scenario <see cref="HttpClient"/>.</summary>
    public void DisposeClient() => _client.Dispose();
}
