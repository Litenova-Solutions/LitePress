namespace LitePress.AcceptanceTests.Support;

/// <summary>
/// Per-scenario scratch pad for Reqnroll steps. Stored in <see cref="Reqnroll.ScenarioContext"/>
/// and reset in <see cref="Hooks.AcceptanceTestHooks.AfterScenario"/>.
/// </summary>
public sealed class ScenarioState
{
    /// <summary>JWT used as the authenticated author for admin API calls.</summary>
    public string? BearerToken { get; set; }

    /// <summary>Id of the post under test in the current scenario.</summary>
    public string? PostId { get; set; }

    /// <summary>Title of the post under test (for slug derivation assertions).</summary>
    public string? PostTitle { get; set; }

    /// <summary>Expected public slug for the post under test.</summary>
    public string? PostSlug { get; set; }

    /// <summary>Id of the tag under test in the current scenario.</summary>
    public string? TagId { get; set; }

    /// <summary>Display name of the tag under test.</summary>
    public string? TagName { get; set; }

    /// <summary>Most recent HTTP response from a step (for Then assertions).</summary>
    public HttpResponseMessage? LastResponse { get; set; }

    /// <summary>Clears scenario fields and disposes <see cref="LastResponse"/>.</summary>
    public void Reset()
    {
        BearerToken = null;
        PostId = null;
        PostTitle = null;
        PostSlug = null;
        TagId = null;
        TagName = null;
        LastResponse?.Dispose();
        LastResponse = null;
    }
}
