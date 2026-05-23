using LiteNova.LitePress.Domain.Shared;

namespace LiteNova.LitePress.Domain.Posts.Events;

public sealed record PostArchived(
    PostId PostId,
    DateTimeOffset ArchivedAt
) : IDomainEvent;
