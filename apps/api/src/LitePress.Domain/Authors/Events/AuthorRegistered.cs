using LitePress.Domain.Shared;

namespace LitePress.Domain.Authors.Events;

public sealed record AuthorRegistered(AuthorId AuthorId, string DisplayName) : IDomainEvent;
