using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Posts.Events;

public sealed record PostArchived(
    PostId PostId,
    DateTimeOffset ArchivedAt
) : IDomainEvent;
