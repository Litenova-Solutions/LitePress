using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LitePress.Integration.Tests.Infrastructure;

namespace LitePress.Integration.Tests.Posts;

[Collection(ApiIntegrationCollection.Name)]
public sealed class PostLifecycleTests
{
    private readonly HttpClient _client;

    public PostLifecycleTests(ApiIntegrationFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task PublishPost_WithTags_PersistsPublishedStateAndPublishedAt()
    {
        var tagId = await CreateTagAsync("integration-tag");
        var postId = await CreateDraftPostAsync("Publish lifecycle test", [tagId]);

        var publishResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, $"/api/posts/{postId}/publish").WithBearer());

        publishResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"/api/posts/{postId}").WithBearer());

        getResponse.IsSuccessStatusCode.Should().BeTrue();
        using var json = await JsonDocument.ParseAsync(await getResponse.Content.ReadAsStreamAsync());
        var root = json.RootElement;

        root.GetProperty("postState").GetString().Should().Be("Published");
        root.GetProperty("publishedAt").GetString().Should().NotBeNullOrEmpty();
        root.GetProperty("tags").GetArrayLength().Should().Be(1);
    }

    [Fact]
    public async Task PublishPost_WithoutTags_PersistsPublishedState()
    {
        var postId = await CreateDraftPostAsync("Publish without tags");

        var publishResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, $"/api/posts/{postId}/publish").WithBearer());

        publishResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"/api/posts/{postId}").WithBearer());

        getResponse.IsSuccessStatusCode.Should().BeTrue();
        using var json = await JsonDocument.ParseAsync(await getResponse.Content.ReadAsStreamAsync());

        json.RootElement.GetProperty("postState").GetString().Should().Be("Published");
        json.RootElement.GetProperty("publishedAt").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ArchivePost_AfterPublish_PersistsArchivedState()
    {
        var postId = await CreateDraftPostAsync("Archive lifecycle test");

        await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, $"/api/posts/{postId}/publish").WithBearer());

        var archiveResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, $"/api/posts/{postId}/archive").WithBearer());

        archiveResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"/api/posts/{postId}").WithBearer());

        using var json = await JsonDocument.ParseAsync(await getResponse.Content.ReadAsStreamAsync());

        json.RootElement.GetProperty("postState").GetString().Should().Be("Archived");
    }

    [Fact]
    public async Task AddTagToDraftPost_PersistsTagAssignment()
    {
        var tagId = await CreateTagAsync("draft-tag");
        var postId = await CreateDraftPostAsync("Add tag test");

        var addTagResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, $"/api/posts/{postId}/tags")
            {
                Content = JsonContent.Create(new { tagId }),
            }.WithBearer());

        addTagResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"/api/posts/{postId}").WithBearer());

        using var json = await JsonDocument.ParseAsync(await getResponse.Content.ReadAsStreamAsync());

        json.RootElement.GetProperty("tags").GetArrayLength().Should().Be(1);
        json.RootElement.GetProperty("tags")[0].GetProperty("tagId").GetString().Should().Be(tagId);
    }

    private async Task<string> CreateDraftPostAsync(string title, IReadOnlyList<string>? tagIds = null)
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
                    tagIds = tagIds ?? [],
                }),
            }.WithBearer());

        createResponse.IsSuccessStatusCode.Should().BeTrue();
        using var json = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        return json.RootElement.GetProperty("postId").GetString()!;
    }

    private async Task<string> CreateTagAsync(string name)
    {
        var createResponse = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, "/api/tags")
            {
                Content = JsonContent.Create(new { name }),
            }.WithBearer());

        createResponse.IsSuccessStatusCode.Should().BeTrue();
        using var json = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        return json.RootElement.GetProperty("tagId").GetString()!;
    }
}
