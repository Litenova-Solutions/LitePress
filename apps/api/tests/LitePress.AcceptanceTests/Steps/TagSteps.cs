using LitePress.AcceptanceTests.Support;

namespace LitePress.AcceptanceTests.Steps;

[Binding]
public sealed class TagSteps
{
    private readonly ScenarioContext _scenarioContext;

    public TagSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private ScenarioState State => _scenarioContext.Get<ScenarioState>();
    private TestApiClient Api => _scenarioContext.Get<TestApiClient>();

    [Given("the author creates a tag named {string}")]
    public async Task GivenTheAuthorCreatesATagNamed(string name)
    {
        State.BearerToken ??= TestUsers.CreateAuthorToken();
        State.TagName = name;

        var response = await Api.CreateTagAsync(name, State.BearerToken);
        State.LastResponse = response;
        response.IsSuccessStatusCode.Should().BeTrue();
        State.TagId = await Api.ReadTagIdAsync(response);
    }

    [When("the author creates another tag named {string}")]
    public async Task WhenTheAuthorCreatesAnotherTagNamed(string name)
    {
        State.TagName = name;
        State.LastResponse = await Api.CreateTagAsync(name, State.BearerToken!);
    }
}
