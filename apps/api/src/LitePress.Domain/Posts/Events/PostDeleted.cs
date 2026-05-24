using LitePress.Domain.Shared;

namespace LitePress.Domain.Posts.Events;

public sealed record PostDeleted(PostId PostId) : IDomainEvent;
