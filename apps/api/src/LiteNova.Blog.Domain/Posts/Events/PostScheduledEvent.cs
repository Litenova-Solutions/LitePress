namespace LiteNova.Blog.Domain.Posts.Events;
public sealed class PostScheduledEvent(Guid PostId, DateTimeOffset ScheduledFor);
