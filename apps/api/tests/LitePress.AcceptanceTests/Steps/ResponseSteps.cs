using System.Net;
using System.Text.Json;
using LitePress.AcceptanceTests.Support;

namespace LitePress.AcceptanceTests.Steps;

[Binding]
public sealed class ResponseSteps
{
    private readonly ScenarioContext _scenarioContext;

    public ResponseSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private ScenarioState State => _scenarioContext.Get<ScenarioState>();
    private TestApiClient Api => _scenarioContext.Get<TestApiClient>();

    [Then("the response is successful")]
    public void ThenTheResponseIsSuccessful()
    {
        State.LastResponse.Should().NotBeNull();
        State.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
    }

    [Then("the response is no content")]
    public void ThenTheResponseIsNoContent()
    {
        State.LastResponse.Should().NotBeNull();
        State.LastResponse!.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Then("the response is unauthorized")]
    public void ThenTheResponseIsUnauthorized()
    {
        State.LastResponse.Should().NotBeNull();
        State.LastResponse!.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Then("the response is not found")]
    public void ThenTheResponseIsNotFound()
    {
        State.LastResponse.Should().NotBeNull();
        State.LastResponse!.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Then("the response is a conflict problem")]
    public async Task ThenTheResponseIsAConflictProblem()
    {
        State.LastResponse.Should().NotBeNull();
        State.LastResponse!.StatusCode.Should().Be(HttpStatusCode.Conflict);
        State.LastResponse.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        using var json = await JsonDocument.ParseAsync(await State.LastResponse.Content.ReadAsStreamAsync());
        json.RootElement.GetProperty("status").GetInt32().Should().Be((int)HttpStatusCode.Conflict);
    }

    [Then("the post is visible in the published posts feed")]
    public async Task ThenThePostIsVisibleInThePublishedPostsFeed()
    {
        var response = await Api.GetPublishedPostsAsync();
        response.IsSuccessStatusCode.Should().BeTrue();

        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var items = json.RootElement.GetProperty("items");
        items.GetArrayLength().Should().BeGreaterThan(0);

        var titles = items.EnumerateArray()
            .Select(item => item.GetProperty("title").GetString())
            .ToList();

        titles.Should().Contain(State.PostTitle);
    }

    [Then("the post is readable by slug on the public API")]
    public async Task ThenThePostIsReadableBySlugOnThePublicApi()
    {
        State.PostSlug.Should().NotBeNullOrEmpty();
        var response = await Api.GetPostBySlugAsync(State.PostSlug!);
        response.IsSuccessStatusCode.Should().BeTrue();

        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        json.RootElement.GetProperty("title").GetString().Should().Be(State.PostTitle);
        json.RootElement.GetProperty("postState").GetString().Should().Be("Published");
    }

    [Then("the tag appears in the tag list")]
    public async Task ThenTheTagAppearsInTheTagList()
    {
        var response = await Api.GetAllTagsAsync();
        response.IsSuccessStatusCode.Should().BeTrue();

        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var names = json.RootElement.EnumerateArray()
            .Select(item => item.GetProperty("name").GetString())
            .ToList();

        names.Should().Contain(State.TagName);
    }
}
