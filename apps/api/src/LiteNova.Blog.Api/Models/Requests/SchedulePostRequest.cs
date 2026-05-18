namespace LiteNova.Blog.Api.Models.Requests;

/// <summary>Request body for scheduling a blog post for future publication.</summary>
public sealed record SchedulePostRequest(DateTimeOffset ScheduledFor);
