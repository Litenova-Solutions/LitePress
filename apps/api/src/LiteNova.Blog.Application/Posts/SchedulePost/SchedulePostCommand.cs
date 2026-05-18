using LiteBus.Commands.Abstractions;
namespace LiteNova.Blog.Application.Posts.SchedulePost;
public sealed record SchedulePostCommand(Guid Id, DateTimeOffset ScheduledFor) : ICommand<SchedulePostResult>;
