using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Commands.SchedulePost;
public sealed record SchedulePostCommand(Guid Id, DateTimeOffset ScheduledFor) : ICommand<SchedulePostResult>;
