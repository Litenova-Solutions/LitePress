using LitePress.AcceptanceTests.Support;

namespace LitePress.AcceptanceTests.Hooks;

/// <summary>
/// Reqnroll lifecycle hooks: one shared database and API host per test run, clean data and fresh clients per scenario.
/// </summary>
[Binding]
public sealed class AcceptanceTestHooks
{
    private static AcceptanceTestWebAppFactory? _factory;

    /// <summary>Starts PostgreSQL Testcontainer, applies Marten schema, and boots the Web API once for all features.</summary>
    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        _factory = new AcceptanceTestWebAppFactory();
        await _factory.InitializeAsync();
    }

    /// <summary>Tears down the Web API host and Testcontainer after all scenarios complete.</summary>
    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (_factory is not null)
        {
            await _factory.DisposeAsync();
            _factory = null;
        }
    }

    /// <summary>Resets Marten data and registers per-scenario support types in <see cref="ScenarioContext"/>.</summary>
    [BeforeScenario]
    public async Task BeforeScenario(ScenarioContext scenarioContext)
    {
        await _factory!.ResetScenarioDataAsync();

        scenarioContext.Set(_factory);
        scenarioContext.Set(new ScenarioState());
        scenarioContext.Set(new TestApiClient(_factory.CreateScenarioClient()));
    }

    /// <summary>Disposes the scenario HTTP client and clears <see cref="ScenarioState"/>.</summary>
    [AfterScenario]
    public void AfterScenario(ScenarioContext scenarioContext)
    {
        if (scenarioContext.TryGetValue(out TestApiClient client))
        {
            client.DisposeClient();
        }

        if (scenarioContext.TryGetValue(out ScenarioState state))
        {
            state.Reset();
        }
    }
}
