namespace LiteNova.Blog.Api.Models.Requests;

/// <summary>
/// Request payload used to schedule a post publication.
/// </summary>
public sealed record SchedulePostRequest
{
    /// <summary>
    /// Date and time when the post should be published.
    /// </summary>
    public required DateTimeOffset ScheduledFor { get; init; }
}
