using LiteNova.LitePress.Domain.Shared;

namespace LiteNova.LitePress.Domain.Authors.Events;

public sealed record AuthorRegistered(AuthorId AuthorId, string DisplayName) : IDomainEvent;
