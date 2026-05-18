using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.SchedulePost;

/// <summary>Command to schedule a blog post for future publication.</summary>
public sealed record SchedulePostCommand(Guid Id, DateTimeOffset ScheduledFor) : ICommand<SchedulePostResult>;
