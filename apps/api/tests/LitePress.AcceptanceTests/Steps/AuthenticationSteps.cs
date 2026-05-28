using LitePress.AcceptanceTests.Support;

namespace LitePress.AcceptanceTests.Steps;

[Binding]
public sealed class AuthenticationSteps
{
    private readonly ScenarioContext _scenarioContext;

    public AuthenticationSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private ScenarioState State => _scenarioContext.Get<ScenarioState>();

    [Given("an authenticated author exists")]
    public void GivenAnAuthenticatedAuthorExists()
    {
        State.BearerToken = TestUsers.CreateAuthorToken();
    }
}
