using LitePress.AcceptanceTests.Support;

namespace LitePress.AcceptanceTests.Steps;

[Binding]
public sealed class PostPublishingSteps
{
    private readonly ScenarioContext _scenarioContext;

    public PostPublishingSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private ScenarioState State => _scenarioContext.Get<ScenarioState>();
    private TestApiClient Api => _scenarioContext.Get<TestApiClient>();

    [Given("the author has a draft post titled {string}")]
    public async Task GivenTheAuthorHasADraftPost(string title)
    {
        State.PostTitle = title;
        State.PostSlug = SlugHelper.PostSlugFromTitle(title);
        State.BearerToken ??= TestUsers.CreateAuthorToken();

        var createResponse = await Api.CreateDraftPostAsync(title, State.BearerToken);
        State.PostId = await Api.ReadPostIdAsync(createResponse);
    }

    [Given("the author has a published post titled {string}")]
    public async Task GivenTheAuthorHasAPublishedPost(string title)
    {
        await GivenTheAuthorHasADraftPost(title);
        State.LastResponse = await Api.PublishPostAsync(State.PostId!, State.BearerToken);
        State.LastResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    [When("the author publishes the post")]
    public async Task WhenTheAuthorPublishesThePost()
    {
        State.LastResponse = await Api.PublishPostAsync(State.PostId!, State.BearerToken);
    }

    [When("an unauthenticated caller publishes the post")]
    public async Task WhenAnUnauthenticatedCallerPublishesThePost()
    {
        State.LastResponse = await Api.PublishPostAsync(State.PostId!, bearerToken: null);
    }
}
