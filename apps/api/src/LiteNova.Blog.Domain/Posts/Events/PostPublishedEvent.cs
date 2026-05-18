using LiteBus.Events.Abstractions;

namespace LiteNova.Blog.Domain.Posts.Events;

public sealed record PostPublishedEvent(Guid PostId) : IEvent;
