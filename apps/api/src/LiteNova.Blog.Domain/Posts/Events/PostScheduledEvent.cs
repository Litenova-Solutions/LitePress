namespace LiteNova.Blog.Domain.Posts.Events;
public sealed record PostScheduledEvent(Guid PostId, DateTimeOffset ScheduledFor);
