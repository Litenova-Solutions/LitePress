using LitePress.Domain.Shared;

namespace LitePress.Domain.Posts.Events;

public sealed record PostArchived(
    PostId PostId,
    DateTimeOffset ArchivedAt
) : IDomainEvent;
