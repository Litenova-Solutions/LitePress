using LitePress.AcceptanceTests.Support;

namespace LitePress.AcceptanceTests.Steps;

[Binding]
public sealed class PostDeleteSteps
{
    private readonly ScenarioContext _scenarioContext;

    public PostDeleteSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private ScenarioState State => _scenarioContext.Get<ScenarioState>();
    private TestApiClient Api => _scenarioContext.Get<TestApiClient>();

    [When("the author deletes the post")]
    public async Task WhenTheAuthorDeletesThePost()
    {
        State.LastResponse = await Api.DeletePostAsync(State.PostId!, State.BearerToken!);
    }

    [When("the author requests the post by id")]
    public async Task WhenTheAuthorRequestsThePostById()
    {
        State.LastResponse = await Api.GetPostByIdAsync(State.PostId!, State.BearerToken!);
    }
}
