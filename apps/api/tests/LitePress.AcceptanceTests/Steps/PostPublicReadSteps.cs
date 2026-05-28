using LitePress.AcceptanceTests.Support;

namespace LitePress.AcceptanceTests.Steps;

[Binding]
public sealed class PostPublicReadSteps
{
    private readonly ScenarioContext _scenarioContext;

    public PostPublicReadSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private ScenarioState State => _scenarioContext.Get<ScenarioState>();
    private TestApiClient Api => _scenarioContext.Get<TestApiClient>();

    [When("an anonymous caller requests the post by slug")]
    public async Task WhenAnAnonymousCallerRequestsThePostBySlug()
    {
        State.PostSlug.Should().NotBeNullOrEmpty();
        State.LastResponse = await Api.GetPostBySlugAsync(State.PostSlug!);
    }
}
