using LiteNova.LitePress.Domain.Shared;

namespace LiteNova.LitePress.Domain.Posts.Events;

public sealed record PostDeleted(PostId PostId) : IDomainEvent;
