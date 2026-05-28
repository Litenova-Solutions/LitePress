using System.Net.Http.Json;
using System.Text.Json;
using LitePress.Domain.Posts;
using Microsoft.Extensions.DependencyInjection;

namespace LitePress.Integration.Tests.Infrastructure;

/// <summary>
/// End-to-end check that HTTP create persists a post Marten can read back through <see cref="IPostRepository"/>.
/// </summary>
[Collection(ApiIntegrationCollection.Name)]
public sealed class PostRepositoryReadTests
{
    private readonly ApiIntegrationFixture _fixture;
    private readonly HttpClient _client;

    public PostRepositoryReadTests(ApiIntegrationFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    /// <summary>POST /api/posts then load the same id from the repository in a new DI scope.</summary>
    [Fact]
    public async Task CreateViaApi_ThenRepositoryGetById_Succeeds()
    {
        var createResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, "/api/posts")
            {
                Content = JsonContent.Create(new
                {
                    title = $"Repo read {Guid.NewGuid():N}",
                    content = "{\"type\":\"doc\",\"content\":[]}",
                    excerpt = "excerpt",
                    tagIds = Array.Empty<string>(),
                }),
            }.WithBearer());

        createResponse.IsSuccessStatusCode.Should().BeTrue(await createResponse.Content.ReadAsStringAsync());
        using var json = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        var postId = new PostId(json.RootElement.GetProperty("postId").GetGuid());

        await using var scope = _fixture.Factory.Services.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
        var post = await repository.GetByIdAsync(postId, CancellationToken.None);
        post.Title.Value.Should().NotBeNullOrEmpty();
    }
}
